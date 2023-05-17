using System;

namespace WPFLibrary.Web.Interfaces;

public interface IHTTPExceptionHandler
{
    void HandleException(Exception exception);
}