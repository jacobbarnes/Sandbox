function getVideoChat() {
    return axios.get('/VideoChat/Index')
        .then(function (response) {
            document.getElementById("mainContent").innerHTML = response.data.view;
            //document.getElementById("videoSource").src = 
        })
        .catch(function (error) { })
}

function changeMainVideo(id) {
    var source = document.getElementById("videoSource");
    var video = document.getElementById("mainVideo");
    source.src = `/VideoChat/GetVideo/bed84e13-0639-47ce-abaf-dfa4ff6e4826/${id}`;
    video.load();
    video.play();

    //change currentVideo
    var els = document.getElementsByClassName("thumbnail");
    for (el of els) {
        el.classList.remove('currentVideo');
    }
    document.getElementById(`thumbnail${id}`).classList.add('currentVideo');

}