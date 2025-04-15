function changeChat(element) {
    const activeChat = document.querySelector(".active");
    activeChat.classList.remove("active");
    element.classList.add("active");
    const messContainer = document.querySelector(".messcont");
    while (messContainer.firstChild) {
        messContainer.removeChild(messContainer.firstChild);
    }
    //Вставить фукнцию загрузки сообщений сюда
}

function sendMess() {
    const txtMes = document.getElementById("messTxt").value;
    const messContainer = document.querySelector(".messcont");
    const newMess = document.createElement("div");
    newMess.classList.add("mess");
    newMess.classList.add("self");
    const newMessTxt = document.createElement("div");
    newMessTxt.innerText = txtMes;
    const newMessTime = document.createElement("div");
    newMessTime.innerText = new Date();
    newMessTxt.classList.add("textMes");
    newMessTime.classList.add("timeMes");
    newMess.appendChild(newMessTxt);
    newMess.appendChild(newMessTime);
    messContainer.appendChild(newMess);
    document.getElementById("messTxt").value = "";
    messContainer.scrollTop = messContainer.scrollHeight;
    //Вставить функцию отправки на сервер
}