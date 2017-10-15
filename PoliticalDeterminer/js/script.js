$(document).ready(function () {

    var facebook = $("#facebook"),
        reddit = $("#reddit"),
        field = $('input[type="text"]'),
        button = $('input[type="submit"]'),
        mainCard = $('#main-card'),
        title = $('#title'),
        footer = $('#footer');

    var cardHeight = mainCard.height();

    $('.socialBtnGroup .socialBtn').click(function(){
        $(this).parent().find('.socialBtn').removeClass('selected');
        $(this).addClass('selected');
        radioVal = $(this).attr('data-value');
        field.attr('placeholder', $(this).attr('hint'));
    });

    button.click(function() {
        if (radioVal != null && field.text() != null && field.val() != "") {
            mainCard.css('min-height', cardHeight);

            var input = field.val(),
                platform = (radioVal == 'fb') ? "facebook" : "reddit",
                site = "http://politicritic.azurewebsites.net/leaning/" + platform + "?user=" + input;

            //Validate formatting
            if (platform == "facebook") {
                var fbRegex = /facebook\.com\/.+/i;
                if (!fbRegex.test(input))
                    return;
            }
            else if (platform == "reddit") {
                var rdRegex = /[^a-zA-Z0-9-_]+/
                if (rdRegex.test(input))
                    return;
            }

            mainCard.children().addClass('fadeOut');
            mainCard.t = setTimeout((function() {
                mainCard.children().remove();
            }), 500);
            //Results revealed here after the button press
            $.get(site, function(data) {
                mainCard.t = setTimeout((function () {
                    data = data * 2 - 1;

                    mainCard.append("<h3 style='opacity: 0.0; transition: opacity 500ms;'>" + data.toFixed(2));

                    if (data.toFixed(2) < 0.0) {
                        mainCard.append("<h3 style='opacity: 0.0; transition: opacity 500ms;'>" + "Liberal Leaning");
                    } else if (data.toFixed(2) > 0.0) {
                        mainCard.append("<h3 style='opacity: 0.0; transition: opacity 500ms;'>" + "Conservative Leaning");
                    } else {
                        mainCard.append("<h3 style='opacity: 0.0; transition: opacity 500ms;'>" + "Perfectly Moderate");
                    }

                    mainCard.append("</h3><p style='opacity: 0.0; transition: opacity 500ms;'>It's our best guess.</p><input style='opacity: 0.0; transition: opacity 500ms;' class='noselect' id='again' type='submit' name='repeat' value='Again?'>");

                    mainCard.children().animate({opacity: 1.0}, 500);
                }), 500);
            });
        }
    });

    $('body').on('click', '#again', function() {
        location.reload();
    });

    //******************Actions******************

    title.t = setTimeout((function() {
        title.css('visibility', 'visible').animate({opacity: 1.0}, {duration:400});
    }), 100);

    mainCard.t = setTimeout((function() {
        mainCard.css('visibility', 'visible').animate({opacity: 1.0, top: '50%'}, {duration:400});
    }), 300);

    footer.t = setTimeout((function() {
        footer.css('visibility', 'visible').animate({opacity: 1.0}, {duration:400});
    }), 500);

    //******************Background******************/

    var colors = new Array(
        [239, 83, 80],
        [211, 47, 47],
        [66, 165, 245],
        [144, 202, 249],
        [25, 118, 210]);

    var step = 0;
    //color table indices for:
    // current color left
    // next color left
    // current color right
    // next color right
    var colorIndices = [0,1,2,3];

    //transition speed
    var gradientSpeed = 0.003;

    function updateGradient()
    {

      if ( $===undefined ) return;

    var c0_0 = colors[colorIndices[0]];
    var c0_1 = colors[colorIndices[1]];
    var c1_0 = colors[colorIndices[2]];
    var c1_1 = colors[colorIndices[3]];

    var istep = 1 - step;
    var r1 = Math.round(istep * c0_0[0] + step * c0_1[0]);
    var g1 = Math.round(istep * c0_0[1] + step * c0_1[1]);
    var b1 = Math.round(istep * c0_0[2] + step * c0_1[2]);
    var color1 = "rgb("+r1+","+g1+","+b1+")";

    var r2 = Math.round(istep * c1_0[0] + step * c1_1[0]);
    var g2 = Math.round(istep * c1_0[1] + step * c1_1[1]);
    var b2 = Math.round(istep * c1_0[2] + step * c1_1[2]);
    var color2 = "rgb("+r2+","+g2+","+b2+")";

     $('body').css({
       background: "-webkit-gradient(linear, left top, right top, from("+color1+"), to("+color2+"))"}).css({
        background: "-moz-linear-gradient(left, "+color1+" 0%, "+color2+" 100%)"});

      step += gradientSpeed;
      if ( step >= 1 )
      {
        step %= 1;
        colorIndices[0] = colorIndices[1];
        colorIndices[2] = colorIndices[3];

        //pick two new target color indices
        //do not pick the same as the current one
        colorIndices[1] = ( colorIndices[1] + Math.floor( 1 + Math.random() * (colors.length - 1))) % colors.length;
        colorIndices[3] = ( colorIndices[3] + Math.floor( 1 + Math.random() * (colors.length - 1))) % colors.length;

      }
    }

    setInterval(updateGradient,10);

});
