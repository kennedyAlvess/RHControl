function showAlert(title, mensagem, tipo) {
    Swal.fire({
        title: title,
        text: mensagem,
        icon: tipo,
        confirmButtonText: 'OK'
    });
}

function showLoading() {
    document.getElementById("loading").style.display = "flex";
}

function hideLoading() {
    document.getElementById("loading").style.display = "none";
}

function confirmarExclusao(button) {
    var href = button.getAttribute('href');
    var match = href.match(/__doPostBack\('([^']*)','([^']*)'\)/);
    console.log(match);
    Swal.fire({
        title: "Voc\u00ea tem certeza?",
        text: "Essa a\u00e7\u00e3o n\u00e3o poder\u00e1 ser desfeita!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Confirmar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            __doPostBack(match[1], match[2]);
        }
    });

    return false;
}
