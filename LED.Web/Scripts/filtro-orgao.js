// -----------------------------------------------------
// URL
// -----------------------------------------------------

URL_CarregaComboNur = '';
URL_CarregaComboComarca = '';
URL_CarregaComboServentia = '';
URL_CarregaComboOutrosOrgaos = '';

// -----------------------------------------------------
// FUNÇÕES QUE PODEM SER SOBRESCRITAS PELO UTILIZADOR
// -----------------------------------------------------

CarregarEmail_Extensao = CarregarEmail_Padrao;
PesquisarPorOutrosOrgaos_Extensao = PesquisarPorOutrosOrgaos_Padrao;
LimparControleOutrosOrgaos_Extensao = LimparControleOutrosOrgaos_Padrao;

// -----------------------------------------------------
// FUNÇÕES EXECUTADAS NO LOAD DA PÁGINA
// -----------------------------------------------------

function FiltroOrgaoLoad() {

    // Controle Nur
    $.ajax({
        url: URL_CarregaComboNur,
        type: "GET",
        dataType: 'json',
        cache: false,
        async: false,
        success: function (obj) {
            if (obj != null) {
                var data = obj.data;
                var selectbox = $('#dropNur');
                selectbox.find('option').remove();
                $('<option>').val(0).text("Selecione...").appendTo(selectbox);
                $.each(data, function (i, d) {
                    $('<option>').val(d.codNurc).text(d.descrRes).appendTo(selectbox);
                });
            }
        }
    });

    $('#dropNur').change(function () {
        fnPesquisarPorFiltro();
        LimparControleServentia();
        LimparControleComarca();
        if ($('#dropNur').val() == "0") {
            DesabilitarControleComarca(true);
            DesabilitarControleServentia(true);
        } else {
            DesabilitarControleComarca(false);
            DesabilitarControleServentia(true);
        }
    });

    // Controle Comarca
    DesabilitarControleComarca(true);

    $("#inputNumeroComarca").inputmask("9999", { "placeholder": " ", "clearIncomplete": false });

    $("#inputNomeComarca").on('keyup', function () {
        $("#inputNumeroComarca").prop('disabled', $(this).val() != "");
    });

    $("#inputNumeroComarca").on('keyup', function () {
        $("#inputNomeComarca").prop('disabled', $(this).val() != "");
    });

    $("#btnLimparInputComarca").click(function () {
        LimparControleServentia();
        LimparControleComarca();
        $("#inputNumeroComarca").focus();
        DesabilitarControleServentia(true);
    });

    $('#selectComarca').change(function () {
        LimparControleServentia();
        DesabilitarControleServentia($(this).val() == "");
    });

    $("#btnPesquisarInputComarca").click(function () {
        var numeroVazio = (isNaN(parseInt($("#inputNumeroComarca").val().trim())) || parseInt($("#inputNumeroComarca").val().trim()) == 0);

        if (numeroVazio && ($("#inputNomeComarca").val().trim() == "")) {
            $("#inputNumeroComarca").focus();
            return;
        } else {
            if (numeroVazio && $("#inputNomeComarca").val().trim().length < 4) {
                $("#inputNomeComarca").focus();
                return;
            }
        }

        $("#secaoListarComarca").show();
        $("#secaoBuscarComarca").hide();

        $.ajax({
            url: URL_CarregaComboComarca,
            type: "GET",
            dataType: 'json',
            cache: false,
            async: false,
            data: {
                'codNur': $('#dropNur').val(),
                'codOrg': $("#inputNumeroComarca").val().trim() == "" ? 0 : $("#inputNumeroComarca").val().trim(),
                'nomeOrg': $("#inputNomeComarca").val().trim()
            },
            success: function (obj) {
                if (obj != null) {
                    var data = obj.data;
                    var selectbox = $('#selectComarca');

                    selectbox.find('option').remove();

                    if (data.length == 0) {
                        $('<option>').val('').text('Não encontrado...').appendTo(selectbox);
                        return;
                    } else if (data.length > 1) {
                        $('<option>').val('').text('Selecione...').appendTo(selectbox);
                    }

                    if ($("#inputNumeroComarca").val() != "") {
                        if (data[0].CodComarca != null) {
                            $('<option>').val(data[0].CodComarca).text(data[0].DescrComarca).appendTo(selectbox);
                        }
                    }
                    else {
                        $.each(data, function (i, d) {
                            $('<option>').val(d.CodComarca).text(d.DescrComarca).appendTo(selectbox);
                        });
                    }

                    if (data.length == 1)
                        selectbox.change();

                    selectbox.focus();
                }
            }
        });

    });

    // Controle Serventia
    DesabilitarControleServentia(true);

    $("#inputNumeroServentia").inputmask("9999", { "placeholder": " ", "clearIncomplete": false });

    $("#inputNomeServentia").on('keyup', function () {
        $("#inputNumeroServentia").prop('disabled', $(this).val() != "");
    });

    $("#inputNumeroServentia").on('keyup', function () {
        $("#inputNomeServentia").prop('disabled', $(this).val() != "");
    });

    $("#btnLimparInputServentia").click(function () {
        LimparControleServentia();
        $("#inputNumeroServentia").focus();
    });

    $("#btnPesquisarInputServentia").click(function () {
        var numeroVazio = (isNaN(parseInt($("#inputNumeroServentia").val().trim())) || parseInt($("#inputNumeroServentia").val().trim()) == 0);

        if (numeroVazio && ($("#inputNomeServentia").val().trim() == "")) {
            $("#inputNumeroServentia").focus();
            return;
        } else {
            if (numeroVazio && $("#inputNomeServentia").val().trim().length < 4) {
                $("#inputNomeServentia").focus();
                return;
            }
        }

        $("#secaoListarServentia").show();
        $("#secaoBuscarServentia").hide();

        $.ajax({
            url: URL_CarregaComboServentia,
            type: "GET",
            dataType: 'json',
            cache: false,
            async: false,
            data: {
                'codNur': $('#dropNur').val(),
                'codCom': $('#selectComarca').val().trim(),
                'codServ': $("#inputNumeroServentia").val().trim() == "" ? 0 : $("#inputNumeroServentia").val().trim(),
                'nomeServ': $("#inputNomeServentia").val().trim()
            },
            success: function (obj) {
                if (obj != null) {
                    var data = obj.data;
                    var selectbox = $('#selectServentia');

                    selectbox.find('option').remove();

                    if (data.length == 0) {
                        $('<option>').val('').text('Não encontrado...').appendTo(selectbox);
                        return;
                    } else if (data.length > 1) {
                        $('<option>').val('').text('Selecione...').appendTo(selectbox);
                    }

                    if ($("#inputNumeroServentia").val() != "") {
                        if (data[0].codOrg != null) {
                            $('<option>').val(data[0].codOrg).text(data[0].nome).appendTo(selectbox);
                        }
                    }
                    else {
                        $.each(data, function (i, d) {
                            $('<option>').val(d.codOrg).text(d.nome).appendTo(selectbox);
                        });
                    }

                    if (data.length == 1)
                        selectbox.change();

                    selectbox.focus();
                }
            }
        });

    });

    $('#selectServentia').change(function () {
        CarregarEmail_Extensao($(this).val());
    });

    // Controle Outros Órgãos
    $("#inputNumeroOutroOrgao").inputmask("9999", { "placeholder": " ", "clearIncomplete": false });

    $("#inputNomeOutroOrgao").on('keyup', function () {
        $("#inputNumeroOutroOrgao").prop('disabled', $(this).val() != "");
    });

    $("#inputNumeroOutroOrgao").on('keyup', function () {
        $("#inputNomeOutroOrgao").prop('disabled', $(this).val() != "");
    });

    $("#btnLimparInputOutroOrgao").click(function () {
        LimparControleOutrosOrgaos();
        LimparControleOutrosOrgaos_Extensao();
        $("#inputNumeroOutroOrgao").focus();
    });

    $("#btnPesquisarInputOutroOrgao").click(function () {
        var numeroVazio = (isNaN(parseInt($("#inputNumeroOutroOrgao").val().trim())) || parseInt($("#inputNumeroOutroOrgao").val().trim()) == 0);

        if (numeroVazio && ($("#inputNomeOutroOrgao").val().trim() == "")) {
            $("#inputNumeroOutroOrgao").focus();
            return;
        } else {
            if (numeroVazio && $("#inputNomeOutroOrgao").val().trim().length < 4) {
                $("#inputNomeOutroOrgao").focus();
                return;
            }
        }

        $("#secaoListarOutroOrgao").show();
        $("#secaoBuscarOutroOrgao").hide();

        $.ajax({
            url: URL_CarregaComboOutrosOrgaos,
            type: "GET",
            dataType: 'json',
            cache: false,
            async: false,
            data: {
                'codOrg': $("#inputNumeroOutroOrgao").val().trim() == "" ? 0 : $("#inputNumeroOutroOrgao").val().trim(),
                'nomeOrg': $("#inputNomeOutroOrgao").val().trim()
            },
            success: function (obj) {
                if (obj != null) {
                    var data = obj.data;
                    var selectbox = $('#selectOutroOrgao');

                    selectbox.find('option').remove();

                    if (data.length == 0) {
                        $('<option>').val('').text('Não encontrado...').appendTo(selectbox);
                        return;
                    } else if (data.length > 1) {
                        $('<option>').val('').text('Selecione...').appendTo(selectbox);
                    }

                    if ($("#inputNumeroOutroOrgao").val() != "") {
                        if (data[0].codOrg != null) {
                            $('<option>').val(data[0].codOrg).text(data[0].nome).appendTo(selectbox);
                        }
                    }
                    else {
                        $.each(data, function (i, d) {
                            $('<option>').val(d.codOrg).text(d.nome).appendTo(selectbox);
                        });
                    }

                    if (data.length == 1)
                        selectbox.change();

                    selectbox.focus();
                }
            }
        });

    });

    $('#selectOutroOrgao').change(function () {
        PesquisarPorOutrosOrgaos_Extensao();
        CarregarEmail_Extensao($(this).val());
    });

}

// -----------------------------------------------------
// FUNÇÃO PARA NUR
// -----------------------------------------------------

function DefinirControleNur(id, nome) {
    var $select = $('#dropNur');
    $select.find('option').remove();
    $('<option>').val(id).text(nome).appendTo($select);
    $select.val(id);
}

function LimparControleNur() {
    $('#dropNur').val(0);
}

function DesabilitarControleNur(disabled) {
    $('#dropNur').prop('disabled', disabled);
}

// -----------------------------------------------------
// FUNÇÃO PARA COMARCA
// -----------------------------------------------------

function DefinirControleComarca(id, nome) {
    $("#secaoBuscarComarca").hide();
    $("#secaoListarComarca").show();

    var $select = $('#selectComarca');
    $select.find('option').remove();
    $('<option>').val(id).text(nome).appendTo($select);
    $select.val(id);
}

function LimparControleComarca() {
    $('#btnLimparInputComarca').prop('disabled', false);
    $('#btnPesquisarInputComarca').prop('disabled', false);
    $("#secaoBuscarComarca").show();
    $("#secaoListarComarca").hide();
    $('#selectComarca').val('').change();
    $("#inputNomeComarca").val("").prop('disabled', false);
    $("#inputNumeroComarca").val("").prop('disabled', false);
}

function DesabilitarControleComarca(disabled) {
    $('#btnLimparInputComarca').prop('disabled', disabled);
    $('#btnPesquisarInputComarca').prop('disabled', disabled);
    $('#selectComarca').prop('disabled', disabled);
    $("#inputNomeComarca").prop('disabled', disabled);
    $("#inputNumeroComarca").prop('disabled', disabled);
}

// -----------------------------------------------------
// FUNÇÃO PARA SERVENTIA
// -----------------------------------------------------

function DefinirControleServentia(id, nome) {
    $("#secaoBuscarServentia").hide();
    $("#secaoListarServentia").show();

    var $select = $('#selectServentia');
    $select.find('option').remove();
    $('<option>').val(id).text(nome).appendTo($select);
    $select.val(id);
}

function LimparControleServentia() {
    $('#btnLimparInputServentia').prop('disabled', false);
    $('#btnPesquisarInputServentia').prop('disabled', false);
    $("#secaoBuscarServentia").show();
    $("#secaoListarServentia").hide();
    $('#selectServentia').val('').change();
    $("#inputNomeServentia").val("").prop('disabled', false);
    $("#inputNumeroServentia").val("").prop('disabled', false);
}

function DesabilitarControleServentia(disabled) {
    $('#btnLimparInputServentia').prop('disabled', disabled);
    $('#btnPesquisarInputServentia').prop('disabled', disabled);
    $('#selectServentia').prop('disabled', disabled);
    $("#inputNomeServentia").prop('disabled', disabled);
    $("#inputNumeroServentia").prop('disabled', disabled);
}

// -----------------------------------------------------
// FUNÇÃO PARA OUTROS ÓRGÃOS
// -----------------------------------------------------

function DefinirControleOutrosOrgaos(id, nome) {
    $("#secaoBuscarOutroOrgao").hide();
    $("#secaoListarOutroOrgao").show();

    var $select = $('#selectOutroOrgao');
    $select.find('option').remove();
    $('<option>').val(id).text(nome).appendTo($select);
    $select.val(id);
}

function LimparControleOutrosOrgaos() {
    $('#btnLimparInputOutroOrgao').prop('disabled', false);
    $('#btnPesquisarInputOutroOrgao').prop('disabled', false);
    $("#secaoBuscarOutroOrgao").show();
    $("#secaoListarOutroOrgao").hide();
    $('#selectOutroOrgao').val('').change();
    $("#inputNomeOutroOrgao").val("").prop('disabled', false);
    $("#inputNumeroOutroOrgao").val("").prop('disabled', false);
}

function DesabilitarControleOutrosOrgaos(disabled) {
    $('#btnLimparInputOutroOrgao').prop('disabled', disabled);
    $('#btnPesquisarInputOutroOrgao').prop('disabled', disabled);
    $('#selectOutroOrgao').prop('disabled', disabled);
    $("#inputNomeOutroOrgao").prop('disabled', disabled);
    $("#inputNumeroOutroOrgao").prop('disabled', disabled);
}

// -----------------------------------------------------
// FUNÇÕES AUXILIARES
// -----------------------------------------------------

function fnLimparDesabilitarOrgaos(disabled) {
    LimparControleNur();
    LimparControleComarca();
    LimparControleServentia();
    LimparControleOutrosOrgaos();
    fnDesabilitarOrgaos(disabled);
}

function fnDesabilitarOrgaos(disabled) {
    DesabilitarControleNur(disabled);
    DesabilitarControleComarca(disabled);
    DesabilitarControleServentia(disabled);
    DesabilitarControleOutrosOrgaos(disabled);
}

function fnPesquisarPorFiltro() {
    var disabled = ($('#dropNur').val() != 0);

    DesabilitarControleOutrosOrgaos(disabled);
}

function PesquisarPorOutrosOrgaos_Padrao() {
    var disabled = ($("#selectOutroOrgao").val() != '' && $("#selectOutroOrgao").val() != null);

    DesabilitarControleNur(disabled);
    DesabilitarControleComarca(disabled);
    DesabilitarControleServentia(disabled);
}

function CarregarEmail_Padrao(o) {
    return;
}

function LimparControleOutrosOrgaos_Padrao() {
    return;
}

function fnObterOrgaoDestino() {
    if ($('#selectOutroOrgao').val() == '' || $('#selectOutroOrgao').val() == null)
        return $('#selectServentia').val();

    return $('#selectOutroOrgao').val();
}