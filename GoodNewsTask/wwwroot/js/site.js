// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Переключение между цветовыми решениями
function selectMode(selectedByUserMode) { //функция выбора цветового режима

    const modesArray = ["morningMode", "dayMode", "eveningMode", "nightMode"]; //инициализация массива с режимами

    modesArray.forEach((anyMode) => {
        document.body.classList.remove(anyMode);
    });//удаление (методом remove()) всех активных (работающих) режимов (в нашем случае берётся body)

    if (selectedByUserMode) {
        document.body.classList.add(selectedByUserMode);
    }//добавление (методом add()) выбранным пользователем режима и его запуск
}

//Реализация каждого цветового режима под каждую кнопку
function morningModeToggle() {
    selectMode("morningMode");//для <body>
    document.getElementById("navbar").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-morning"; //отдельно для элементов с id="navbar"
    document.getElementById("navbar2").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-morning"; //отдельно для элементов с id="navbar2"
}

function dayModeToggle() {
    selectMode("dayMode");//для <body>
    document.getElementById("navbar").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-day"; //отдельно для элементов с id="navbar"
    document.getElementById("navbar2").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-day"; //отдельно для элементов с id="navbar2"
}

function eveningModeToggle() {
    selectMode("eveningMode");//для <body>
    document.getElementById("navbar").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-evening"; //отдельно для элементов с id="navbar"
    document.getElementById("navbar2").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-evening"; //отдельно для элементов с id="navbar2"
}
function nightModeToggle() {
    selectMode("nightMode");//для <body>
    document.getElementById("navbar").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-night"; //отдельно для элементов с id="navbar"
    document.getElementById("navbar2").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-night"; //отдельно для элементов с id="navbar2"
}