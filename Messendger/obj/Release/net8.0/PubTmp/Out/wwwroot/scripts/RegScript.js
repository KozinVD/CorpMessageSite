document.getElementById("inputBtn").addEventListener("click", RedirectToInput);
document.getElementById("formReg").addEventListener("submit", Check);

function RedirectToInput(){
    location.assign("/");
}

function Check(e) {
    e.preventDefault();
    const pasTxt = document.getElementById("password");
    const reppasTxt = document.getElementById("reppassword");

    if (!pasTxt.value || !reppasTxt.value) {
        alert("Пожалуйста, заполните все поля");
        return;
    }

    if (pasTxt.value !== reppasTxt.value) {
        alert("Пароль не совпадает");
        return;
    }

    if (validatePassword(pasTxt.value).length !== 0) {
        pasTxt.setCustomValidity(validatePassword(pasTxt.value));
        return;
    }
    // Если все проверки пройдены, отправляем форму
    e.target.submit();
}
function checkPass(element) {
    const pass = element.value;
    const list = document.getElementById("errorlist");
    list.innerHTML = "";
    if (validatePassword(pass).length !== 0) {
        validatePassword(pass).forEach(error => {
            list.innerHTML += `<li class="list-group-item text-danger bg-dark">${error}</li>`;
        });
    }
}

function validatePassword(password) {
    const errors = [];

    // Проверка длины
    if (password.length < 6) {
        errors.push("Пароль должен содержать минимум 6 символов");
    }

    // Проверка на наличие строчной буквы
    if (!/[a-z]/.test(password)) {
        errors.push("Пароль должен содержать хотя бы одну строчную букву (a-z)");
    }

    // Проверка на наличие заглавной буквы
    if (!/[A-Z]/.test(password)) {
        errors.push("Пароль должен содержать хотя бы одну заглавную букву (A-Z)");
    }

    // Проверка на наличие не буквенно-цифрового символа
    if (!/[^a-zA-Z0-9]/.test(password)) {
        errors.push("Пароль должен содержать хотя бы один специальный символ");
    }

    // Формирование сообщения с перечислением всех ошибок
    return errors;
}