
$('#Description').keyup(function () {
   // var err = ValidateDescription();
})
$('#Cost').keyup(function () {
    var err = ValidateCost();
})

$('#create-inventory-submit').click(function (e) {
    e.preventDefault();
    var errors = null;
    if (IsInputEmpty('Description'))
    {
        errors = 'err'
    }
    errors += ValidateCost();
    console.log(errors);
    if (errors === 0) {
        $('#CreateInventoryItemForm').submit();
    }
})

function ValidateCost() {
    if (!$.isNumeric($('#Cost').val())) {
        $('#cost-error').html('Cost must be a number');
        return 'err';
    }
    else {
        $('#cost-error').html('');
        return null;
    }
}

//must have an id that matches the error message 
//must have error span with id="id + -error"
$('.text-input-validation').keyup(function (e) {
    var me = $(this);
    var id = me.attr('id');
    if(!IsInputEmpty(id))
    {
        $('#' + id + '-error').html('');
    }

})

function IsInputEmpty(elementId)
{
    if ($('#' + elementId).val().trim() === '') {
        HandleEmptyInputErrors(elementId);
        return true;
    }
    return false;
}

function HandleEmptyInputErrors(elementId)
{
    $('#' + elementId + '-error').html(elementId + ' cannot be empty');
}


$('numberic-input-validation').keyup(function (e) {

})