
function islocahost() {
    return location.hostname.toLowerCase() == "localhost";
}

function lenpath() {
    return location.pathname.split('/').length;
}

function baseUrl() {
    return (lenpath() == 3 && !islocahost()) ? "" : lenpath() == 2 ? "" : "../";
}

function Logout() {
    
    $.ajax({
        url: baseUrl() + 'Base/Logout',
        type: 'GET',
        success: function (response) {
            if (response) {
                window.document.location.href = response;
            }
        }
    });

}

