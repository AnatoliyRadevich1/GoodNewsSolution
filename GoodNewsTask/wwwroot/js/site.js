﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Переключение между цветовыми решениями
function selectMode(selectedByUserMode) { //функция выбора цветового режима

    const modesArray = ["morningMode", "dayMode", "eveningMode", "nightMode"]; //инициализация массива с режимами

    modesArray.forEach((anyMode) => {
        document.body.classList.remove(anyMode);
        document.getElementById("navbar").classList.remove(anyMode); //ПРОВЕРЯЮ, НАДО СПРОСИТЬ ПОЧЕМУ ОТВАЛИЛСЯ ФОН ДЛЯ NAVBAR-ОВ
        document.getElementById("navbar2").classList.remove(anyMode); //ПРОВЕРЯЮ, НАДО СПРОСИТЬ ПОЧЕМУ ОТВАЛИЛСЯ ФОН ДЛЯ NAVBAR-ОВ
    });//удаление (методом remove()) всех активных (работающих) режимов (в нашем случае берётся body)

    if (selectedByUserMode) {
        document.body.classList.add(selectedByUserMode);
        document.getElementById("navbar").classList.add(selectedByUserMode); //ПРОВЕРЯЮ, НАДО СПРОСИТЬ ПОЧЕМУ ОТВАЛИЛСЯ ФОН ДЛЯ NAVBAR-ОВ
        document.getElementById("navbar2").classList.add(selectedByUserMode); //ПРОВЕРЯЮ, НАДО СПРОСИТЬ ПОЧЕМУ ОТВАЛИЛСЯ ФОН ДЛЯ NAVBAR-ОВ
        localStorage.setItem('selectedMode', selectedByUserMode); //ПРОВЕРЯЮ, НАДО СПРОСИТЬ ПОЧЕМУ ОТВАЛИЛСЯ ФОН ДЛЯ NAVBAR-ОВ
    }//добавление (методом add()) выбранным пользователем режима и его запуск
}

document.addEventListener('DOMContentLoaded', (event) => { //ПРОВЕРЯЮ, НАДО СПРОСИТЬ ПОЧЕМУ ОТВАЛИЛСЯ ФОН ДЛЯ NAVBAR-ОВ
    const savedMode = localStorage.getItem('selectedMode'); //ПРОВЕРЯЮ, НАДО СПРОСИТЬ ПОЧЕМУ ОТВАЛИЛСЯ ФОН ДЛЯ NAVBAR-ОВ
    if (savedMode) { //ПРОВЕРЯЮ, НАДО СПРОСИТЬ ПОЧЕМУ ОТВАЛИЛСЯ ФОН ДЛЯ NAVBAR-ОВ
        selectMode(savedMode);//ПРОВЕРЯЮ, НАДО СПРОСИТЬ ПОЧЕМУ ОТВАЛИЛСЯ ФОН ДЛЯ NAVBAR-ОВ
    }
});

//Реализация каждого цветового режима под каждую кнопку
function morningModeToggle() {
    selectMode("morningMode");//для <body> 
    
    //document.getElementById("navbar").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-morning border-bottom box-shadow mb-3"; //отдельно для элементов с id="navbar"
    //document.getElementById("navbar2").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-morning border-bottom box-shadow mb-3"; //отдельно для элементов с id="navbar2"
}

function dayModeToggle() {
    selectMode("dayMode");//для <body>
    //document.getElementById("navbar").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-day border-bottom box-shadow mb-3"; //отдельно для элементов с id="navbar"
    //document.getElementById("navbar2").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-day border-bottom box-shadow mb-3"; //отдельно для элементов с id="navbar2"
}

function eveningModeToggle() {
    selectMode("eveningMode");//для <body>
    //document.getElementById("navbar").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-evening border-bottom box-shadow mb-3"; //отдельно для элементов с id="navbar"
    //document.getElementById("navbar2").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-evening border-bottom box-shadow mb-3"; //отдельно для элементов с id="navbar2"
}
function nightModeToggle() {
    selectMode("nightMode");//для <body>
    //document.getElementById("navbar").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-night border-bottom box-shadow mb-3"; //отдельно для элементов с id="navbar"
    //document.getElementById("navbar2").className = "navbar navbar-expand-sm navbar-toggleable-sm navbar-night border-bottom box-shadow mb-3"; //отдельно для элементов с id="navbar2"
}