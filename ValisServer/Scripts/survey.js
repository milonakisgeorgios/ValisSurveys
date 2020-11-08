$(function () {

    $("input.Integer, input.Date_Year, input.Date_Day, input.Date_Month").keydown(function (evt) {
        if (evt.keyCode == 173 || evt.keyCode == 189/*-,keyboard*/ || evt.keyCode == 109)/*-, numeric*/ {
            var _value = $(evt.target).val();
            if (_value.length > 0)
                evt.preventDefault();
            if (_value.indexOf("-") != -1)
                evt.preventDefault();
            return;
        }
        if (evt.keyCode == 37 || evt.keyCode == 39)/*arrows*/ {
            return;
        }
        if (evt.keyCode == 8/*Backspace*/ || evt.keyCode == 9/*TAB*/ || evt.keyCode == 35/*End*/ || evt.keyCode == 36 /*Home*/ || evt.keyCode == 46/*Delete*/ ) {
            return;
        }
        //number from keypad
        if (evt.keyCode <= 105 && evt.keyCode >= 96) {
            return;
        }
        //number from keyboard
        if (evt.keyCode <= 57 && evt.keyCode >= 48) {
            return;
        }
        //Allow: Ctrl+A
        if (evt.keyCode == 65 && evt.ctrlKey === true)
            return;
        evt.preventDefault();
    });

    $("input.Decimal").keydown(function (evt) {
        /*το κόμμα είναι το 188*/
        if (evt.keyCode == 190/*., keyboard*/ || evt.keyCode == 110/*., numeric*/) {
            var _value = $(evt.target).val();
            if (_value.indexOf(".") != -1)
                evt.preventDefault();
            return;
        }
        if (evt.keyCode == 173 || evt.keyCode == 189/*-,keyboard*/ || evt.keyCode == 109)/*-, numeric*/ {
            var _value = $(evt.target).val();
            if (_value.length > 0)
                evt.preventDefault();
            if (_value.indexOf("-") != -1)
                evt.preventDefault();
            return;
        }
        if (evt.keyCode == 37 || evt.keyCode == 39)/*arrows*/ {
            return;
        }
        if (evt.keyCode == 8/*Backspace*/ || evt.keyCode == 9/*TAB*/ || evt.keyCode == 35/*End*/ || evt.keyCode == 36 /*Home*/ || evt.keyCode == 46/*Delete*/ ) {
            return;
        }
        //number from keypad
        if (evt.keyCode <= 105 && evt.keyCode >= 96) {
            return;
        }
        //number from keyboard
        if (evt.keyCode <= 57 && evt.keyCode >= 48) {
            return;
        }
        //Allow: Ctrl+A
        if (evt.keyCode == 65 && evt.ctrlKey === true)
            return;
        evt.preventDefault();
    });


    $('input.Date1').datepicker({
        buttonImage: "/content/images/datepicker-20.png",
        showOn: "both",
        buttonImageOnly: true,
        dateFormat: "mm/dd/yy"
    });
    $('input.Date2').datepicker({
        buttonImage: "/content/images/datepicker-20.png",
        showOn: "both",
        buttonImageOnly: true,
        dateFormat: "dd/mm/yy"
    });



});


function OnNext() {

}

function OnPrevious() {

}