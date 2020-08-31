using FaceRecognitionDotNet;
using foiEPP.Data;
using foiEPP.Models;
using foiEPP.Viewmodels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace foiEPP.Helpers
{
    public class FaceRecognitionHelper
    {
        const double tolerance = 0.6d;

        string modelDirectory;
        private readonly FacultyContext _context;
        public FaceRecognitionHelper(FacultyContext context)
        {
            modelDirectory = Path.Combine(Environment.CurrentDirectory, @"FaceRecognitionModels");
            _context = context;
        }

        public List<StudentWithImageViewModel> RecognizeStudents(string imagePath)
        {
            List<StudentWithImageViewModel> recognizedStudents = new List<StudentWithImageViewModel>();

            // get student encodings
            List<UserImages> students = _context.UserImages.ToList();
            List<FaceEncoding> knownStudents = new List<FaceEncoding>();
            foreach(UserImages student in students)
            {
                var bf = new BinaryFormatter();
                using (var ms2 = new MemoryStream(student.Encoding))
                {
                    var loaded = bf.Deserialize(ms2) as FaceEncoding;
                    knownStudents.Add(loaded);
                }
            }
            IEnumerable<FaceEncoding> knownEncodings = knownStudents;

            // match known encodings with loaded ones
            using (FaceRecognition fr = FaceRecognition.Create(modelDirectory))
            {
                using (FaceRecognitionDotNet.Image image = FaceRecognition.LoadImageFile(imagePath))
                {
                    IEnumerable<Location> locations = fr.FaceLocations(image);
                    IEnumerable<FaceEncoding> encodings = fr.FaceEncodings(image, locations);
                    foreach(var encoding in encodings)
                    {
                        List<bool> matches = FaceRecognition.CompareFaces(knownEncodings, encoding, tolerance).ToList();
                        if (matches.Contains(true))
                        {
                            for(int i = 0; i < matches.Count(); i++)
                            {
                                if (matches[i])
                                {
                                    StudentWithImageViewModel student = new StudentWithImageViewModel();
                                    Bitmap face = Crop(imagePath, locations.ToList()[i].Bottom, locations.ToList()[i].Left, locations.ToList()[i].Right, locations.ToList()[i].Top);
                                    ImageConverter converter = new ImageConverter();
                                    student.Image = (byte[])converter.ConvertTo(face, typeof(byte[]));
                                    student.Student = _context.Users.Where(student => student.ID == students[i].UserID).First();
                                    recognizedStudents.Add(student);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return recognizedStudents;
        }

        private Bitmap Crop(string myImage, int bottom, int left, int right, int top)
        {
            Bitmap croppedBitmap = new Bitmap(myImage);
            croppedBitmap = croppedBitmap.Clone(
                            new Rectangle(left, top, right-left, bottom-top),
                            System.Drawing.Imaging.PixelFormat.DontCare);
            return croppedBitmap;
        }

        public bool AddNewFaces()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FaceRecognition.InternalEncoding = Encoding.UTF8;
            string[] facesDirectory = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, @"Faces"), "*.jpg", SearchOption.AllDirectories);
            using (FaceRecognition fr = FaceRecognition.Create(modelDirectory))
            {
                foreach(string path in facesDirectory)
                {
                    using (FaceRecognitionDotNet.Image image = FaceRecognition.LoadImageFile(path))
                    {
                        IEnumerable<Location> locations = fr.FaceLocations(image);
                        IEnumerable<FaceEncoding> encodings = fr.FaceEncodings(image, locations);
                        if (encodings.Count() > 0) {
                            UserImages faceImage = new UserImages();
                            var bf = new BinaryFormatter();
                            using (var ms1 = new MemoryStream())
                            {
                                bf.Serialize(ms1, encodings.First());
                                ms1.Flush();

                                faceImage.Encoding = ms1.ToArray();
                            }
                            string[] studentName = Path.GetFileName(Path.GetDirectoryName(path)).Split(" ");
                            User student = _context.Users.Where(user => user.FirstNameEncoded == studentName[0] && user.LastNameEncoded == studentName[1]).FirstOrDefault();
                            faceImage.UserID = student.ID;
                            _context.UserImages.Add(faceImage);
                            _context.SaveChanges();
                        }
                    }
                }
            }
            return true;
        }
    }
}
