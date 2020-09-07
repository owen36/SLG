using System;
using Unisave.Examples.PlayerAuthentication.Backend;
using Unisave.Facades;
using UnityEngine;
using UnityEngine.UI;

namespace Unisave.Examples.PlayerAuthentication
{
    public class RegistrationController : MonoBehaviour
    {
        public InputField emailInputField;
        public InputField passwordInputField;
        public InputField passwordRepeatInputField;
        public Button registerButton;
        
        private void Start()
        {
            if (emailInputField == null)
                throw new ArgumentNullException(
                    nameof(emailInputField),
                    nameof(LoginController) + " field has not been linked."
                );
            
            if (passwordInputField == null)
                throw new ArgumentNullException(
                    nameof(passwordInputField),
                    nameof(LoginController) + " field has not been linked."
                );
            
            if (passwordRepeatInputField == null)
                throw new ArgumentNullException(
                    nameof(passwordRepeatInputField),
                    nameof(LoginController) + " field has not been linked."
                );
            
            if (registerButton == null)
                throw new ArgumentNullException(
                    nameof(registerButton),
                    nameof(LoginController) + " field has not been linked."
                );
            
            registerButton.onClick.AddListener(RegisterButtonClicked);
        }

        private async void RegisterButtonClicked()
        {
            if (passwordInputField.text != passwordRepeatInputField.text)
            {
                RegistrationFailed("Passwords don't match.");
                return;
            }
            
            var result = await OnFacet<AuthFacet>
                .CallAsync<AuthFacet.RegistrationResult>(
                    nameof(AuthFacet.Register),
                    emailInputField.text,
                    passwordInputField.text
                );

            switch (result)
            {
                case AuthFacet.RegistrationResult.Ok:
                    RegistrationSucceeded();
                    break;
                
                case AuthFacet.RegistrationResult.EmailTaken:
                    RegistrationFailed("Email is already registered.");
                    break;
                
                case AuthFacet.RegistrationResult.InvalidEmail:
                    RegistrationFailed("Provided email is not valid.");
                    break;
                
                case AuthFacet.RegistrationResult.WeakPassword:
                    RegistrationFailed("Provided password is too weak.");
                    break;
                
                default:
                    RegistrationFailed("Unknown error.");
                    break;
            }
        }

        private void RegistrationSucceeded()
        {
            // implement your own logic here
            
            Debug.Log("Hooray! You are now registered *AND ALSO* logged in.");
        }

        private void RegistrationFailed(string message)
        {
            // implement your own logic here
            
            Debug.LogError("Registration failed! " + message);
        }
    }
}