const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

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

function sendMess(e) {
    e.preventDefault();
    const messContainer = document.querySelector(".messcont");
    const UserName = document.getElementById("userName").innerText;
    const txtMes = document.getElementById("messTxt").value;
    document.getElementById("messTxt").value = "";
    messContainer.scrollTop = messContainer.scrollHeight;
    //Вставить функцию отправки на сервер
    hubConnection.invoke("Send", txtMes, UserName) // отправка данных серверу
        .catch(function (err) {
            return console.error(err.toString());
        });
}

hubConnection.on("Receive", function (message, userName) {
    console.log("Новое сообщение!");
    const messContainer = document.querySelector(".messcont");
    const newMess = document.createElement("div");
    newMess.classList.add("mess");
    const UserName = document.getElementById("userName").innerText;
    if (UserName === userName)
        newMess.classList.add("self");
    const newMessTxt = document.createElement("div");
    newMessTxt.innerText = userName + " " + message;
    const newMessTime = document.createElement("div");
    const data = new Date();
    newMessTime.innerText = data.toLocaleTimeString() + " " + data.toLocaleDateString();
    newMessTxt.classList.add("textMes");
    newMessTime.classList.add("timeMes");
    newMess.appendChild(newMessTxt);
    newMess.appendChild(newMessTime);
    messContainer.appendChild(newMess);
});

hubConnection.start()
    .then(function () {
        console.log("Успешно");
    });