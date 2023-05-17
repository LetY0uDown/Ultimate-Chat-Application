using System;
using System.Windows;
using WPFLibrary.Web.Interfaces;

namespace Client.Core;

public class RequestErrorHandler : IHTTPExceptionHandler
{
    public void HandleException (Exception exception)
    {
        MessageBox.Show(exception.Message);
    }
}