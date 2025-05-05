//Настройка подключения
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();
//Запуск подключения к серверу
hubConnection.start()
    .then(function () {
        console.log("Успешно");
    });

//Функиця изменения чата и прогрузки сообщения
async function changeChat(element) {
    const id = element.dataset.idChat;
    const activeChat = document.querySelector(".active");
    if (activeChat !== null) {
        const activeId = activeChat.dataset.idChat;
        if (activeId === id)
            return;
        activeChat.classList.remove("active");
    }
    element.classList.add("active");
    const messContainer = document.querySelector(".messcont");
    messContainer.innerHTML = "";
    //Фукнция загрузки сообщений
    await fetch(`/api/message/${id}`)
        .then(response => response.json())
        .then((messages) => {
            messages.forEach((message) => {
                const newMess = document.createElement("div");
                newMess.classList.add("mess");
                if (message.isYou)
                    newMess.classList.add("self");
                const date = new Date(message.timeSend)
                newMess.innerHTML = `<div class="userMes">${message.sender}</div>
                <div class="textMes">${message.text}</div>
                <div class="timeMes">${date.getHours()}:${date.getMinutes()} ${date.toLocaleDateString()}</div>`;
                messContainer.appendChild(newMess);
            });
        });
    document.getElementById("messTxt").removeAttribute("disabled");
    document.getElementById("sendBtn").removeAttribute("disabled");
    document.getElementById("fileSend").removeAttribute("disabled");
    
    const state = { url: `/ChatMenu/${id}`, title: "Chats", description: "Chat Page" }
    history.replaceState(state, state.title, state.url);
    messContainer.scrollTop = messContainer.scrollHeight;
}

//Функция отправки сообщения
function sendMess(e) {
    e.preventDefault();
    const forma = document.getElementById("formSend");
    const filelist = document.getElementById("fileSend");
    const activeElement = document.querySelector(".active");
    const chatId = activeElement.dataset.idChat;
    const messContainer = document.querySelector(".messcont");
    const userElment = document.getElementById("userName");
    let name = userElment.innerText;
    name = name.substring(0, name.lastIndexOf(" "));
    const txtMes = document.getElementById("messTxt").value;
    document.getElementById("messTxt").value = "";
    const timeMess = new Date();
    if (filelist.files[0]) {
        const formData = new FormData();
        formData.append("fileSend", filelist.files[0]);
        formData.append("textMessage", txtMes);
        fetch(`/sendFiles/${chatId}`, {
            method: 'POST',
            body: formData
        }).then((res) => { res.text(); console.log(res.status) }).then(text => console.log(text));
        return;
    }
    //Функция отправки на сервер
    hubConnection.invoke("Send", chatId, txtMes, `${timeMess.getHours()}:${timeMess.getMinutes()} ${timeMess.toLocaleDateString()}`)
        .catch(function (err) {
            return console.error(err.toString());
        });
    const newMess = document.createElement("div");
    newMess.classList.add("mess");
    newMess.classList.add("self");
    newMess.innerHTML = `<div class="userMes">${name}</div><div class="textMes">${txtMes}</div><div class="timeMes">${timeMess.getHours()}:${timeMess.getMinutes()} ${timeMess.toLocaleDateString()}</div>`;
    messContainer.appendChild(newMess);
    messContainer.scrollTop = messContainer.scrollHeight;
}

//Функция принятия сообщения
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
    messContainer.scrollTop = messContainer.scrollHeight;
});
hubConnection.on("ReceiveFileY", function (idChat, sender, message, timeMessage) {
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
    newMess.classList.add("self");
    newMess.innerHTML = `<div class="userMes">${sender}</div><div class="textMes">${message}</div><div class="timeMes">${timeMessage}</div>`;
    messContainer.appendChild(newMess);
    messContainer.scrollTop = messContainer.scrollHeight;
});

//Функция создания чата
hubConnection.on("createChat", function (idChat, nameChat, isGroup, path) {
    console.log(`${idChat} ${nameChat} ${isGroup} ${path} `)
    const chatlist = document.getElementById("listChat");
    if (isGroup) {
        chatlist.innerHTML += `<button type="button" data-id-chat="${idChat}" class="list-group-item list-group-item-action d-flex align-items-center mb-2" onclick="changeChat(this)">
        ${nameChat}
        <span class="badge bg-danger rounded-pill ms-auto kolmess"></span>
        </button>`
        return;
    }
    chatlist.innerHTML += `<button type="button" data-id-chat="${idChat}" class="list-group-item list-group-item-action d-flex align-items-center mb-2" onclick="changeChat(this)">
        <img src="${path}" class="me-3" width="75px">
        ${nameChat}
        <span class="badge bg-danger rounded-pill ms-auto kolmess"></span>
        </button>`
    isClick = false;
});
//Функция создаия чата и его открытие
hubConnection.on("createChatO", function (idChat) {
    location.assign(`/ChatMenu/${idChat}`);
    isClick = false;
});

function exitClick() {
    location.assign("/");
}
let timer;

//Функция поиска соотрудникаов
function searchPeople(element) {
    const searchListElement = document.getElementById("searchList");
    clearTimeout(timer);
    const getValue = element.value;
    timer = setTimeout(() => {
        if (searchListElement)
        searchListElement.innerHTML = " ";
        fetch(`/api/users/${getValue}`).then(response => response.json()).then((jsons) => {
            jsons.forEach((user) => {
                const item = document.createElement("li");
                item.classList.add("list-group-item");
                item.classList.add("mb-1");
                item.classList.add("border");
                item.classList.add("border-warning");
                item.innerHTML = `<div class="d-flex align-items-center">
                                <img class="me-3" width="75px" src="${user.path}">
                                <div class="me-3 d-flex flex-column">
                                    <span class="row">${user.fullName}</span>
                                    <span class="row">${user.jobName}</span>
                                </div>
                                <button class="btn border-danger ms-auto me-2" onclick="addFriend(this)" data-user="${user.id}"  type="button">+</button>
                                <button class="btn border border-danger" type="button" onclick="chatbtn(this)" data-user="${user.id}" data-chat="${user.idChat}"><img style="width: 20px;" alt="Смс" src="/res/image/mess.svg"/></button>
                            </div>`;
                searchListElement.appendChild(item);

            });
        });

    }, 1000);
    
}
let isClick = false;
//Функция открытия или создания,если его ещё нет личного чата
function chatbtn(element) {
    if (isClick)
        return;
    isClick = true;
    const idChat = element.dataset.chat;
    const idUser = element.dataset.user;
    if (idChat != "0") {
        location.assign(`/ChatMenu/${idChat}`);
        return;
    }
    let users = [idUser];
    hubConnection.invoke("CreateNewChat", users)
        .catch(function (err) {
            return console.error(err.toString());
        });

}
function changeimg(element) {
    const file = element.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById("previewImg").src = e.target.result;
        };
        reader.readAsDataURL(file);
    }
}

function addFriend(element) {
    const but = element;
    const friendId = but.dataset.user;
    but.innerText = "✓";
    but.setAttribute("disabled", "");
    fetch(`/api/addFr/${friendId}`).then(response => response.text).then(text => console.log(text));
}
function changeList(element) {
    const friendList = document.getElementById("friendList");
    const btn = element;
    const elId = btn.id;
    if (elId === "btnFriend") {
        friendList.innerHTML = "";
        fetch("/api/getFriend").then(response => response.json())
            .then((jsons) => {
                jsons.forEach((json) => {
                    friendList.innerHTML += `<li class="list-group-item mb-1  border border-warning">
                            <div class="d-flex align-items-center">
<img class="me-3" width="75px" src="${json.photoPath}">
<div class="me-3 d-flex flex-column">
<span class="row">${json.surname} ${json.name} ${json.lastname}</span>
 <span class="row">${json.jobName}</span></div>
 <button class="btn border border-danger ms-auto me-2" type="button" onclick="chatbtn(this)" data-user="${json.id}" data-chat="${json.idChat}"><img style="width: 20px;" alt="Смс" src="/res/image/mess.svg" /></button>
  <button class="btn border border-danger" type="button" onclick="delbtnfr(this)" data-user="${json.id}"><img style="width: 20px;" alt="Удалить" src="/res/image/trash.svg"/></button>
 </div>
</li>`;
                });
            });
        return;
    }
    if (elId === "btnVxodReq") {
        friendList.innerHTML = "";
        fetch("/api/getreqv").then(response => response.json())
            .then((jsons) => {
                jsons.forEach((json) => {
                    friendList.innerHTML += `<li class="list-group-item mb-1  border border-warning">
                            <div class="d-flex align-items-center">
<img class="me-3" width="75px" src="${json.photoPath}">
<div class="me-3 d-flex flex-column">
<span class="row">${json.surname} ${json.name} ${json.lastname}</span>
 <span class="row">${json.jobName}</span></div>
 <button class="btn border border-danger ms-auto me-2" type="button" onclick="chatbtn(this)" data-user="${json.id}" data-chat="${json.idChat}" ><img style="width: 20px;"  alt="Смс" src="/res/image/mess.svg" /></button>
   <button class="btn border border-danger me-1" type="button"  onclick="delbtnv(this)" data-user="${json.id}"><img style="width: 20px;" alt="Удалить" src="/res/image/trash.svg"/></button>
    <button class="btn border border-danger" type="button" onclick="acceptbtn(this)"  data-user="${json.id} ">✓</button>
 </div>
</li>`;
                });
            });
        return;
    }
    if (elId === "btnIsxReq") {
        friendList.innerHTML = "";
        fetch("/api/getreqi").then(response => response.json())
            .then((jsons) => {
                jsons.forEach((json) => {
                    friendList.innerHTML += `<li class="list-group-item mb-1  border border-warning">
                            <div class="d-flex align-items-center">
<img class="me-3" width="75px" src="${json.photoPath}">
<div class="me-3 d-flex flex-column">
<span class="row">${json.surname} ${json.name} ${json.lastname}</span>
 <span class="row">${json.jobName}</span></div>
 <button class="btn border border-danger ms-auto me-2" type="button" onclick="chatbtn(this)" data-user="${json.id}" data-chat="${json.idChat}" ><img style="width: 20px;" alt="Смс" src="/res/image/mess.svg" /></button>
   <button class="btn border border-danger" type="button" onclick="delbtnIs(this)" data-user="${json.id}"><img style="width: 20px;"  alt="Удалить" src="/res/image/trash.svg"/></button>
 </div>
</li>`;
                });
            });
        return;
    }
}

async function acceptbtn(element) {
    const but = element;
    const idFrient = but.dataset.user;
    const newBtn = document.getElementById("btnFriend");
    await fetch(`api/addFr/${idFrient}`).then(response => response.text()).then(text => console.log(text));
    newBtn.click();
}

async function delbtnfr(element) {
    const friendList = document.getElementById("friendList");
    const idFrient = element.dataset.user;
    const liElement = element.closest('li');
    console.log(idFrient);
    await fetch(`/api/delFriend/${idFrient}`).then(response => response.text()).then(text => console.log(text));
    friendList.removeChild(liElement);
}
async function delbtnv(element) {
    const friendList = document.getElementById("friendList");
    const idFrient = element.dataset.user;
    const liElement = element.closest('li');
    console.log(idFrient);
    await fetch(`/api/delReqV/${idFrient}`).then(response => response.text()).then(text => console.log(text));
    friendList.removeChild(liElement);
}
async function delbtnIs(element) {
    const friendList = document.getElementById("friendList");
    const idFrient = element.dataset.user;
    const liElement = element.closest('li');
    console.log(idFrient);
    await fetch(`/api/delReqI/${idFrient}`).then(response => response.text()).then(text => console.log(text));
    friendList.removeChild(liElement);
}

function filesend(element) {
    const label = document.getElementById("filebtn");
    const file = element.files[0];
    if (!file)
        return;
    const cancelbtn = document.getElementById("canbtn");
    cancelbtn.classList.remove("d-none");
    label.classList.add("d-none");
}

function cancelbtn(element) {
    const filebut = document.getElementById("filebtn");
    document.getElementById("fileSend").value = '';
    filebut.classList.remove("d-none");
    element.classList.add("d-none");
}