

function getApp() {
    return axios.get('/Home/LoadContent')
        .then(function (response) {
            document.body.innerHTML = response.data.view;
        })
        .catch(function (error) { })
}

function login() {
    return axios.post('/Home/Login', new FormData(document.getElementById("loginForm")))
        .then(function (response) {
            document.body.innerHTML = response.data.view;
        })
        .catch(function (error) { })
}

function logout() {
    return axios.get('/Home/LogOut')
        .then(function (response) {
            document.body.innerHTML = response.data.view;
        })
        .catch(function (error) { })
}

function getChat() {
    return axios.get('/Dashboard/GetChat')
        .then(function (response) {
            document.getElementById("mainContent").innerHTML = response.data.view;
        })
        .catch(function (error) { })
}

function getProfile() {
    return axios.get('/Dashboard/GetProfile')
        .then(function (response) {
            document.getElementById("mainContent").innerHTML = response.data.view;
        })
        .catch(function (error) { })
}