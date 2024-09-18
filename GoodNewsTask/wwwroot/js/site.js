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
    selectMode("morningMode");
}

function dayModeToggle() {
    selectMode("dayMode");
}

function eveningModeToggle() {
    selectMode("eveningMode");
}
function nightModeToggle() {
    selectMode("nightMode");
}