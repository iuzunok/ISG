$(function ()
{
    $(".anchorOku").click(function ()
    {
        // debugger;
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        var Durum = $buttonClicked.attr('data-durum');
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: URLOku,
            contentType: "application/json; charset=utf-8",
            data: { "Durum": Durum, "Key": id },
            datatype: "json",
            success: function (data)
            {
                // debugger;
                $('#argemModalContent').html(data);
                // alert($('#myModalContent').html());
                $('#argemModal').modal(options);
                $('#argemModal').modal('show');

            },
            error: function (request, status, error)
            {
                alert('Sistem Hatası:\n' + request.responseText);
            }
        });
    });

    $(".anchorGuncelle").click(function ()
    {
        // debugger;
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        var Durum = $buttonClicked.attr('data-durum');
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: URLOku,
            contentType: "application/json; charset=utf-8",
            data: { "Durum": Durum, "Key": id },
            datatype: "json",
            success: function (data)
            {
                // debugger;
                $('#argemModalContent').html(data);
                // alert($('#myModalContent').html());
                $('#argemModal').modal(options);
                $('#argemModal').modal('show');

            },
            error: function (request, status, error)
            {
                alert('Sistem Hatası:\n' + request.responseText);
            }
        });
    });
    /*$("#closbtn").click(function ()
    {
        $('#myModal').modal('hide');
    });*/
});
