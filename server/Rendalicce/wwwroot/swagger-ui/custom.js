(function () {
    function wait(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    // Listen for login response and extract the token
    const originalFetch = window.fetch;
    window.fetch = function () {
        return originalFetch.apply(this, arguments).then(function (response) {
            // Intercept the response of the login API (adjust the endpoint if needed)
            if (response.url.endsWith("/auth") && response.status === 200) {
                response.clone().json().then(async function (data) {
                    // Assuming the JWT token is in the 'token' field of the response JSON
                    const token = data.token;
                    if (token) {
                        let openAuthFormButton = document.querySelector(".auth-wrapper .authorize");
                        openAuthFormButton.click();

                        await wait(200);

                        // If the button has the 'locked' class, the user is logged in
                        if (openAuthFormButton.classList.contains("locked")) {
                            console.log("logging out")
                            const logoutButton = document.querySelector(".modal-btn.auth.button");
                            console.log(logoutButton)
                            if (logoutButton) {
                                // Click the logout button
                                logoutButton.click();
                                await wait(100);
                            }
                        }
                        
                        let tokenInput = document.querySelector(".auth-container input");
                        let authButton = document.querySelector(".auth-btn-wrapper .modal-btn.auth");
                        let closeButton = document.querySelector("button.btn-done");

                        let nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, "value").set;
                        nativeInputValueSetter.call(tokenInput, token);

                        let inputEvent = new Event('input', {bubbles: true});
                        tokenInput.dispatchEvent(inputEvent);
                        authButton.click();
                        closeButton.click();
                    }
                });
            }
            
            return response;
        });
    };
})();