function Mascaras() {
    $('.cep').mask('99999-999', { clearIfNotMatch: true });
    $('.telefone').mask('(99) 99999-9999');
}

$(document).ready(function () {
    Mascaras();
});
