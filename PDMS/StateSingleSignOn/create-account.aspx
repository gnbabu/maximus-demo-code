<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="create-account.aspx.cs" Inherits="StateSingleSignOn.create_account" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>PNM Account Creation</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f5f5f5;
        }

        .container {
            max-width: 900px;
            margin: 0 auto;
            background-color: #ffffff;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }

        header {
            background-color: #0056A1;
            padding: 15px;
            color: white;
            text-align: left;
        }

        .content {
            display: flex;
        }

        nav {
            width: 350px;
            padding: 20px;
            background-color: #f1f1f1;
            border-right: 1px solid #ddd;
        }

        nav ul {
            list-style-type: none;
            padding: 0;
        }

        nav ul li {
            padding: 10px 0;
            color: #0056A1;
        }

        nav ul li.active {
            color: black;
        }

        .form-section {
            flex-grow: 1;
        }

        h1 {
            font-size: 24px;
            color: #0056A1;
        }

        input[type="email"] {
            width: calc(100% - 22px);
            padding: 10px;
            margin: 10px 0;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        button {
            background-color: #0056A1;
            color: white;
            border: none;
            padding: 10px 20px;
            cursor: pointer;
            border-radius: 4px;
            font-size: 16px;
        }

            button:disabled {
                background-color: #aaa;
            }

        footer {
            background-color: #f5f5f5;
            padding: 20px;
            text-align: center;
            font-size: 12px;
            color: #555;
        }

            footer p {
                margin: 0;
            }

        .container {
            max-width: 1000px;
            margin: 50px auto;
            background-color: white;
            border-radius: 8px;
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.2);
            overflow: hidden;
        }

        .header {
            background-color: #003366;
            padding: 20px;
            color: white;
            text-align: left;
            font-size: 24px;
            font-weight: bold;
        }

        .form-container {
            padding: 30px;
        }

            .form-container h2 {
                color: #003366;
                margin-bottom: 20px;
            }

            .form-container p {
                color: #666;
                line-height: 1.5;
                margin-bottom: 15px;
            }

        .form-group {
            margin-bottom: 20px;
        }

            .form-group label {
                font-weight: bold;
                display: block;
                margin-bottom: 5px;
            }

            .form-group input[type="email"] {
                width: calc(100% - 22px);
                padding: 10px;
                border: 1px solid #ccc;
                border-radius: 4px;
                font-size: 16px;
            }

                .form-group input[type="email"]:focus {
                    border-color: #003366;
                    outline: none;
                }

        .button-group {
            text-align: right;
        }

            .button-group button {
                background-color: #003366;
                color: white;
                padding: 10px 20px;
                border: none;
                border-radius: 4px;
                font-size: 16px;
                cursor: pointer;
                margin-left: 10px;
            }

                .button-group button:disabled {
                    background-color: #ccc;
                    cursor: not-allowed;
                }

        .cancel {
            background-color: transparent;
            color: #003366;
            border: none;
            cursor: pointer;
        }

            .cancel:hover {
                text-decoration: underline;
            }

        .footer {
            padding: 20px;
            background-color: #f9f9f9;
            border-top: 1px solid #ddd;
            font-size: 12px;
            color: #555;
        }

            .footer a {
                color: #003366;
                text-decoration: none;
            }

        .sidebar {
            background-color: #f9f9f9;
            padding: 20px;
            border-right: 1px solid #ddd;
            width: 300px;
        }

        .sidebar ul {
            list-style: none;
            padding: 0;
            margin: 0;
        }

        .sidebar ul li {
            padding: 15px 0;
            border-bottom: 1px solid #ddd;
            color: #003366;
            cursor: pointer;
        }

        .sidebar ul li.active {
            color: #14499e;
            font-weight: bold;
        }

        .content {
            padding: 20px;
            width: 100%;
        }

        .main-container {
            display: flex;
        }

        .hidden {
            display: none;
        }

        button {
            background-color: #004687;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 1rem;
        }

        .button:hover {
            background-color: #002855;
        }

        .text-small {
            font-size: 0.9rem;
            color: #555;
        }

            .text-small a {
                color: #004687;
                text-decoration: none;
            }

                .text-small a:hover {
                    text-decoration: underline;
                }

        .links {
            margin-top: 30px;
        }

            .links a {
                text-decoration: none;
                color: #004687;
                margin-right: 15px;
            }

                .links a:hover {
                    text-decoration: underline;
                }

        .wrapper {
            display: flex;
        }

        p {
            font-size: 1rem;
            margin-bottom: 10px;
        }

        input[type="text"] {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            margin-bottom: 20px;
            box-sizing: border-box;
        }

        .row {
            display: flex;
            justify-content: space-between;
            gap: 20px;
        }

            .row div {
                flex: 1;
            }

        button {
            background-color: #004687;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 1rem;
        }

            button:hover {
                background-color: #002855;
            }

        .text-small {
            font-size: 0.9rem;
            color: #555;
        }

        .links {
            margin-top: 30px;
            text-align: center;
        }

            .links a {
                text-decoration: none;
                color: #004687;
                margin-right: 15px;
            }

                .links a:hover {
                    text-decoration: underline;
                }

        .wrapper {
            display: flex;
        }

        .divider {
            border-top: 1px solid #ddd;
            margin: 30px 0;
        }

        .password-row {
            display: flex;
            justify-content: space-between;
            gap: 10px;
        }

            .password-row input[type="password"] {
                flex: 1;
            }

        input[type="password"] {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            margin-bottom: 20px;
            box-sizing: border-box;
        }

        .mobile-number-row {
            display: flex;
            align-items: center;
        }

        .terms-link {
            font-size: 0.9rem;
            color: #004687;
        }

            .terms-link:hover {
                text-decoration: underline;
            }

        .mobile-number-row button {
            width: 120px;
        }

        .captcha-question {
            font-size: 1rem;
            margin-top: 30px;
        }

        .captcha-container {
            display: flex;
            align-items: center;
            margin-top: 10px;
        }

        .captcha-container input[type="text"] {
            width: calc(100% - 150px);
            margin-right: 10px;
        }

        .captcha-container button {
            width: 120px;
        }

        .step-verification.active {
            border-radius: 50%;
            background-color: #14499e;
            color: #f6f6f6;
        }

       .step-verification {
            width: 32px;
            height: 32px;
            font-family: Source Sans Pro;
            font-size: 20px;
            color: #f6f6f6;
            text-decoration: none solid #f6f6f6;
            line-height: 34px;
            text-transform: uppercase;
            text-align: center;
            margin-right: 16px;
            display: inline-block;
            border-radius: 50%;
            background-color: #ffffff;
            color: #14499e;
            border: 1px solid gray;
        }

       .linkPNM {
            color: white;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <div class="container">
        <header>
            <h1><a class="linkPNM" href="/MESC_DEMO/Account/Login.aspx">PNM</a></h1>
        </header>
        <div class="content">
            <nav>
                <div class="sidebar">
                    <ul>
                        <li id="step1" class="active" onclick="showStep(1)"><span id="span1" class="step-verification active"> 1 </span>Email Verification</li>
                        <li id="step2" onclick="showStep(2)"><span id="span2" class="step-verification"> 2 </span>Personal Info</li>
                        <li id="step3" onclick="showStep(3)"><span id="span3" class="step-verification"> 3 </span>Pick a Username</li>
                        <li id="step4" onclick="showStep(4)"><span id="span4" class="step-verification"> 4 </span>Create Password</li>
                        <li id="step5" onclick="showStep(5)"><span id="span5" class="step-verification"> 5 </span>Account Recovery</li>
                        <li id="step6" onclick="showStep(6)"><span id="span6" class="step-verification"> 6 </span>Terms & Conditions</li>
                    </ul>
                </div>
            </nav>
            <div class="form-section">
                <div id="content1" class="form-container">
                    <h1>Email Verification</h1>
                    <p>With one State | ID account, you can sign in to multiple State agency systems more securely.</p>
                    <p>
                        You need an active email address to create an State | ID account. Need to create one? Companies such as 
                    <a href="https://google.com" target="_blank">Google</a>, 
                    <a href="https://aol.com" target="_blank">AOL</a>, and 
                    <a href="https://yahoo.com" target="_blank">Yahoo</a> offer free email accounts.
                    </p>
                    <p>We need to verify the email address you want to use for your State | ID account.</p><p>A one-time PIN will be emailed to the email address you provide below.</p>

                    <form>
                        <label for="email">Email Address</label>
                        <input type="email" id="email" name="email" required>

                        <label for="confirm-email">Confirm Email Address</label>
                        <input type="email" id="confirm-email" name="confirm-email" required>

                        <button type="submit" id="send-pin" onclick="showStep(7)" disabled>Send PIN</button>
                    </form>
                </div>
                <div id="content7" class="form-container">
                    <!-- Main content -->
                    <div class="main-content">
                        <h1>Email Verification</h1>
                        <p>An email with a one-time PIN was sent to rohitnagvenkar03@gmail.com.</p>
                        <label for="pin">Enter PIN</label>
                        <br>
                        <input type="text" id="pin" name="pin" value="">
                        <br>
                        <button onclick="verifyPin()">Verify</button>
                        <p class="text-small">
                            Having Trouble?
                    <ul>
                        <li>Search your junk mail and spam folder for an email from: DONOTREPLY-EnterpriseIdentity@maximus.com.</li>
                        <li>Wait 10 minutes and refresh your email inbox.</li>
                    </ul>
                        </p>
                        <p class="text-small">
                            Still Having Trouble?
                    <ul>
                        <li>Your email provider is likely marking this email as spam, which is blocking or delaying it.</li>
                        <li>Add DONOTREPLY-EnterpriseIdentity@maximus.com to your contacts.</li>
                        <li>Ask your IT administrator to add this email to the safe-sender list.</li>
                    </ul>
                        </p>

                        <div class="links">
                            <a href="#" onclick="sendNewPin()">Send me a new PIN</a>
                            <a href="#" onclick="cancel()">Cancel</a>
                        </div>
                    </div>
                </div>
                <div id="content2" class="form-container">
                    <!-- Main content -->
                    <div class="main-content">
                        <h1>Personal Info</h1>
                        <div class="row">
                            <div>
                                <label for="first-name">Legal First Name</label>
                                <input type="text" id="first-name" name="first-name">
                            </div>
                            <div>
                                <label for="last-name">Legal Last Name</label>
                                <input type="text" id="last-name" name="last-name">
                            </div>
                        </div>

                        <div class="row">
                            <div>
                                <label for="dob">Date of Birth</label>
                                <input type="text" id="dob" name="dob" placeholder="mm/dd/yyyy">
                            </div>
                            <div>
                                <label for="ssn">Last 4 digits of SSN (optional)</label>
                                <input type="text" id="ssn" name="ssn">
                            </div>
                        </div>

                        <p class="text-small">Be sure to use your real date of birth, you may need it for account recovery later.</p>

                        <div class="divider"></div>

                        <div class="links">
                            <a href="#" onclick="cancel()">Cancel</a>
                            <button onclick="nextStep()">Next</button>
                        </div>
                    </div>
                </div>
                <div id="content3" class="form-container">
                    <!-- Main content -->
                    <div class="main-content">
                        <h1>Pick a Username</h1>
                        <ul>
                            <li>Must be between 6-64 characters</li>
                            <li>Cannot start or end in a special character</li>
                            <li>Cannot contain only numbers</li>
                            <li>Only . _ - or @ No other special characters</li>
                        </ul>

                        <label for="username">Username</label>
                        <input type="text" id="username" name="username">

                        <div class="divider"></div>

                        <div class="links">
                            <a href="#" onclick="cancel()">Cancel</a>
                            <button onclick="nextStep()">Next</button>
                        </div>
                    </div>
                </div>
                <div id="content4" class="form-container">
                    <!-- Main content -->
                    <div class="main-content">
                        <h1>Create Password</h1>
                        <ul>
                            <li>Must have at least 12 and no more than 30 characters in length</li>
                            <li>Must contain 1 character from each of the following categories:
                        <ul>
                            <li>Upper case letters (A-Z)</li>
                            <li>Lower case letters (a-z)</li>
                            <li>Numbers (0-9)</li>
                            <li>Special characters (!, $, #, %, etc.)</li>
                        </ul>
                            </li>
                            <li>Cannot include your first name, last name, username, or State | ID
                        <ul>
                            <li>Example: If your name or username is John Smith, your password cannot contain “John” or “Smith”</li>
                        </ul>
                            </li>
                        </ul>

                        <div class="password-row">
                            <input type="password" id="password" name="password" placeholder="Password">
                            <input type="password" id="confirm-password" name="confirm-password" placeholder="Confirm Password">
                        </div>

                        <div class="divider"></div>

                        <div class="links">
                            <a href="#" onclick="cancel()">Cancel</a>
                            <button onclick="nextStep()">Next</button>
                        </div>
                    </div>

                </div>
                <div id="content5" class="form-container">
                    <!-- Main content -->
                    <div class="main-content">
                        <h1>Account Recovery</h1>
                        <p>Your email (<strong>mes...@maximus.com</strong>) is the main way you'll reset your password. Adding your mobile number to your account ensures that we have a way to reach you if you lose access to your email.</p>

                        <h3>Set up mobile/text message account recovery</h3>
                        <p>You will receive a PIN via text message. Message and data rates apply. <a href="#" class="terms-link">See Terms & Conditions and Privacy Policies.</a></p>

                        <div class="mobile-number-row">
                            <input type="text" id="mobile-number" placeholder="___-___-____">
                        </div>
                        
                            <button onclick="sendPIN()">Send PIN</button>
                        <p>If you choose not to add your mobile number to your account at this time, you can <a href="#" class="terms-link">skip this step.</a></p>

                        <div class="divider"></div>

                        <div class="links">
                            <a href="#" onclick="cancel()">Cancel</a>
                            <button onclick="nextStep()">Next</button>
                        </div>
                    </div>
                </div>
                <div id="content6" class="form-container">
                    <!-- Main content -->
                    <div class="main-content">
                        <h1>Terms & Conditions</h1>
                        <p>In order to proceed with this request, you must agree to the following terms and conditions.</p>
                        <p>By clicking "I Agree" and creating an State | ID Citizen, Business, or Workforce profile you consent to use electronic signatures with the State and receive communications in electronic form.</p>
                        <p>If you use this site, you are responsible for maintaining the confidentiality of your State | ID account(s) and password(s) and for restricting access to your computer, and you agree to accept responsibility for all activities that occur under your State | ID account(s) or password(s). The State Department of Administrative Services reserves the right, in the event of a violation of law or State policy, or as a result of any suspicious activity occurring on your State | ID account, to refuse service, terminate accounts, remove or edit content on state.maximus.com, or cancel transactions related to your State | ID account..</p>
                        <p>Children under the age of 13 are not eligible to use services that require the submission of personal information and should not submit any personal information to us. This includes submitting personal information to the website as part of a user profile or profile personalization. If you are a child under the age of 13, you can use these services only if used together with your parents or guardians. Ask permission from your parents or guardians if you are under the age of 13.</p>

                        <label class="checkbox-label">
                            <input type="checkbox" id="agree-checkbox">
                            I Agree
                        </label>

                        <div class="captcha-question">
                            <strong>Confirm you are not a robot</strong><br>
                            Bee, chin, ankle, leg and dog: how many body parts in the list?
                        </div>

                        <div class="captcha-container">
                            <input type="text" id="captcha-answer" placeholder="Enter your answer">
                        </div>
                        
                            <button onclick="verifyCaptcha()">Verify</button>
                        <div class="divider"></div>

                        <div class="links">
                            <a href="#" onclick="cancel()">Cancel</a>
                            <button id="create-account-btn" onclick="createAccount()" disabled>Create Account</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <footer>
        <p>Keeping Your Information Safe</p>
        <p>State | ID respects your privacy. All the data we collect is to give you a better and more secure service. State | ID does not lease, sell, or release your information to private companies, contractors, or vendors for any purpose.</p>
    </footer>


    <script>
        const emailInput = document.getElementById('email');
        const confirmEmailInput = document.getElementById('confirm-email');
        const sendPinButton = document.getElementById('send-pin');

        emailInput.addEventListener('input', validateEmails);
        confirmEmailInput.addEventListener('input', validateEmails);

        function validateEmails() {
            if (emailInput.value && confirmEmailInput.value && emailInput.value === confirmEmailInput.value) {
                sendPinButton.disabled = false;
            } else {
                sendPinButton.disabled = true;
            }
        }

        window.onload = function () {
            showStep(1);
        };

        function showStep(step) {
            // Hide all content
            document.querySelectorAll('.form-container').forEach(function (content) {
                content.classList.add('hidden');
            });

            // Remove active class from all steps
            document.querySelectorAll('.sidebar ul li span').forEach(function (stepLi) {
                stepLi.classList.remove('active');
            });

            // Show selected step content and make menu item active
            document.getElementById('content' + step).classList.remove('hidden');

            if (step != '7') {
                document.getElementById('step' + step).classList.add('active');
                document.getElementById('span' + step).classList.add('step-verification', 'active');
            } else {
                document.getElementById('step' + 1).classList.add('active');
                document.getElementById('span' + 1).classList.add('step-verification', 'active');
            }
        }
        function verifyPin() {
            alert('PIN Verified!');
        }

        function sendNewPin() {
            alert('A new PIN has been sent to your email!');
        }

        function nextStep() {
            alert('Proceeding to the next step.');
        }

        function sendPIN() {
            const mobileNumber = document.getElementById("mobile-number").value;

            if (mobileNumber === "") {
                alert("Please enter your mobile number.");
                return;
            }

            alert("PIN sent to " + mobileNumber + ".");
        }

        function nextStep() {
            const password = document.getElementById("password").value;
            const confirmPassword = document.getElementById("confirm-password").value;

            if (password === "" || confirmPassword === "") {
                alert("Please fill in both password fields.");
                return;
            }

            if (password !== confirmPassword) {
                alert("Passwords do not match.");
                return;
            }

            alert("Passwords match. Proceeding to the next step.");
        }

        function cancel() {
            alert('Cancelled!');
        }
    </script>
</body>
</html>
