alert("javascript!!!");

$(document).ready(function () {
            alert("javascript");
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!

            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            var today = yyyy + '-' + mm + '-' + dd;

            $.ajax({
                type: "GET",
                url: "/GiftRegister/Home/GetCurrentName",
                data: "",
                contenttype: "application/json; charset=utf-8",
                datatype: "json",
                success: function(data) {
                   
                    $("#receiveName").val(data);
                    $("#giveName").val(data);
                    $("#receiveDate").val(today);
                    $("#giveDate").val(today);
                },
                failure: function(response) {

                }
            });

            $('body').on('click', '#receivebtn', function() {
                $('#basic').modal('show');
                setTimeout(function() {
                    ////$("#basic").hide();
                    $("#basic .close").click()
                    //window.location.reload()
                    $("body").load(window.location.href);
                }, 500);

            });

            $('body').on('click', '#givebtn', function() {
                $('#basic').modal('show');
                setTimeout(function() {
                    //$("#basic").hide();
                    $("#basic .close").click()
                    //window.location.reload()
                    $("body").load(window.location.href);
                }, 500);
            });

        });
