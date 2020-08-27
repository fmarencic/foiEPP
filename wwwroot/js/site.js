// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



document.getElementsByClassName("addNew")[0].addEventListener("click", function () {
    var elements = document.getElementsByClassName("name");
    var count = elements.length - 1;
    var newNameInput = document.createElement("input");
    var newSurnameInput = document.createElement("input");
    newNameInput.setAttribute("id", "Students_" + count + "__FirstName");
    newSurnameInput.setAttribute("id", "Students_" + count + "__LastName");
    newNameInput.setAttribute("class", "name form-control");
    newSurnameInput.setAttribute("class", "surname form-control");
    newNameInput.setAttribute("name", "Students[" + count + "].FirstName");
    newSurnameInput.setAttribute("name", "Students[" + count + "].LastName");
    newNameInput.setAttribute("type", "text");
    newSurnameInput.setAttribute("type", "text");
    var newRow = document.getElementById("addHere").insertRow();
    var newCell = newRow.insertCell(0);
    var newCell2 = newRow.insertCell(1);
    newCell2.append(newNameInput);
    var newCell3 = newRow.insertCell(2);
    newCell3.append(newSurnameInput);
})

