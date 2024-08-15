(function() {
    window.addEventListener("load",
        function() {
            setTimeout(function() {
                // Section 01 - Set logo
                const logo = document.getElementsByClassName("link");
                logo[0].style.marginLeft = "20px";
                // logo[0].children[0].style.transform = 'scale(1.5)';
                logo[0].children[0].alt = "Natural Slim Logo";
                logo[0].children[0].src = "/swagger-ui/resources/logo.png";
            });
        });
})()