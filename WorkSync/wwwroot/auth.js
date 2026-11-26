
document.addEventListener("DOMContentLoaded", function() {

    // SHOW / HIDE PASSWORD
    document.querySelectorAll(".toggle-password").forEach(function(button) {
        button.addEventListener("click", function() {
            const wrapper = button.closest(".password-wrapper");
            const passwordInput = wrapper?.querySelector("input");

            if (!passwordInput) return;

            const isPassword = passwordInput.type === "password";

            passwordInput.type = isPassword ? "text" : "password";
            button.textContent = isPassword ? "HIDE" : "SHOW";
            button.setAttribute(
                "aria-label",
                isPassword ? "Hide password" : "Show password"
            );
        });
    });

    // DISPLAY MESSAGE
    function showMessage(message, success = false) {
        const messageBox = document.getElementById("message");

        if (!messageBox) return;

        messageBox.textContent = message;
        messageBox.classList.add("visible");

        messageBox.style.background = success ? "#E8F3E9" : "#FFF0ED";
        messageBox.style.color = success ? "#34734A" : "#A33F35";
    }

    // EMPLOYEE / ADMIN LOGIN
    const loginForm = document.getElementById("loginForm");

    if (loginForm) {
        loginForm.addEventListener("submit", async function(event) {
            event.preventDefault();

            const button = loginForm.querySelector(
                'button[type="submit"]'
            );

            const formData = new FormData(loginForm);

            const username = formData.get("username");
            const password = formData.get("password");
            const role = loginForm.dataset.role;

            if (!username || !password || !role) {
                showMessage("Please complete all fields.");
                return;
            }

            button.disabled = true;
            button.textContent = "Signing in...";

            try {
                const response = await fetch("/api/login", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        username: username,
                        password: password,
                        role: role
                    })
                });

                const data = await response.json();

                if (!response.ok) {
                    showMessage(
                        data.message || "Invalid username or password."
                    );
                    return;
                }

                showMessage(data.message, true);

                window.location.href = data.redirect;

            } catch (error) {
                showMessage(
                    "Unable to connect to the server. Please try again."
                );
            } finally {
                button.disabled = false;
                button.innerHTML = 'Sign In <span aria-hidden="true">→</span>';
            }
        });
    }

    // EMPLOYEE REGISTRATION
    const registerForm = document.getElementById("registerForm");

    if (registerForm) {
        registerForm.addEventListener("submit", async function(event) {
            event.preventDefault();

            const formData = new FormData(registerForm);

            const fullName = formData.get("fullName");
            const username = formData.get("username");
            const password = formData.get("password");
            const confirmPassword = formData.get("confirmPassword");

            if (!fullName || !username || !password || !confirmPassword) {
                showMessage("Please complete all fields.");
                return;
            }

            if (password !== confirmPassword) {
                showMessage("Passwords do not match.");
                return;
            }

            const button = registerForm.querySelector(
                'button[type="submit"]'
            );

            button.disabled = true;
            button.textContent = "Creating account...";

            try {
                const response = await fetch("/api/register", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        fullName: fullName,
                        username: username,
                        password: password
                    })
                });

                const data = await response.json();

                if (!response.ok) {
                    showMessage(
                        data.message || "Registration failed."
                    );
                    return;
                }

                showMessage(data.message, true);

                window.location.href = data.redirect;

            } catch (error) {
                showMessage(
                    "Unable to connect to the server. Please try again."
                );
            } finally {
                button.disabled = false;
                button.textContent = "Create Account";
            }
        });
    }

});
