
// Limpa DIV de alerta
function fnLimparAlerta() {
    $("#divAlerta").empty();
}

function exibeAlerta(tipo, mensagem) {
    var node = "";
    if (tipo == "sucesso") {
        node += '<div class="alert alert-success" role="alert"><i class="fa fa-check"></i>&nbsp;&nbsp;&nbsp;' + mensagem + '</div>';
    }
    else if (tipo == "erro") {
        node += '<div class="alert alert-danger" role="alert"><i class="fa fa-close"></i>&nbsp;&nbsp;&nbsp;' + mensagem + '</div>';
    }
    $("#divAlerta").append(node);
}

//function bloquearTelaImprimir() {
//    $.blockUI({
//        css: {
//            border: 'none',
//            padding: '15px',
//            backgroundColor: '#000',
//            '-webkit-border-radius': '10px',
//            '-moz-border-radius': '10px',
//            opacity: .5,
//            color: '#fff'
//        }
//    });
//}

//function desbloquearTelaImprimir() {
//    setTimeout($.unblockUI, 1000);
//}