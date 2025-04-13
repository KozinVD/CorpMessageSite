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

    // Если все проверки пройдены, отправляем форму
    e.target.submit();
}