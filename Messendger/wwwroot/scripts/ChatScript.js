const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

function changeChat(element) {
    const activeChat = document.querySelector(".active");
    if (activeChat !== null)
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
    const activeElement = document.querySelector(".active");
    const chatId = activeElement.dataset.idChat;
    const messContainer = document.querySelector(".messcont");
    const userElment = document.getElementById("userName");
    const userId = userElment.getAttribute("data-idUser");
    let name = userElment.innerText;
    console.log(name.lastIndexOf(" "));
    name = name.substring(0, name.lastIndexOf(" "));
    console.log(name);
    const txtMes = document.getElementById("messTxt").value;
    document.getElementById("messTxt").value = "";
    messContainer.scrollTop = messContainer.scrollHeight;
    //Функция отправки на сервер
    const timeMess = new Date();
    hubConnection.invoke("Send", chatId, userId, txtMes, `${timeMess.getHours()}:${timeMess.getMinutes()} ${timeMess.toLocaleDateString()}`)
        .catch(function (err) {
            return console.error(err.toString());
        });
    const newMess = document.createElement("div");
    newMess.classList.add("mess");
    newMess.classList.add("self");
    newMess.innerHTML = `<div class="userMes">${name}</div><div class="textMes">${txtMes}</div><div class="timeMes">${timeMess.getHours()}:${timeMess.getMinutes()} ${timeMess.toLocaleDateString()}</div>`;
    messContainer.appendChild(newMess);
}

hubConnection.on("Receive", function (idChat, sender, message, timeMessage) {
    const activeElement = document.querySelector(".active");
    let chatId = 0;
    if (activeElement != null)
    chatId = activeElement.dataset.idChat;
    if (chatId != idChat) {
        const chat = document.querySelector(`[data-id-chat="${idChat}"]`);
        const UvedElement = chat.querySelector(".kolmess");
        const current = parseInt(UvedElement.innerText);
        UvedElement.innerText = (isNaN(current) ? 1 : current + 1);
        return;
    }
    const messContainer = document.querySelector(".messcont");
    const newMess = document.createElement("div");
    newMess.classList.add("mess");
    newMess.innerHTML = `<div class="userMes">${sender}</div><div class="textMes">${message}</div><div class="timeMes">${timeMessage}</div>`;
    messContainer.appendChild(newMess);
});

hubConnection.start()
    .then(function () {
        console.log("Успешно");
    });