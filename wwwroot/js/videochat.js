function getVideoChat() {
    return axios.get('/VideoChat/Index')
        .then(function (response) {
            document.getElementById("mainContent").innerHTML = response.data.view;
            //document.getElementById("videoSource").src = 
        })
        .catch(function (error) { })
}