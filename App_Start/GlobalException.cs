using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.App_Start
{
    public class GlobalException
    {
        public static string GetException(string errorMessage, string strackTrace, string username, string InnerException, string GetClassName, string URL)
        {
            CarProjectEntities db = new CarProjectEntities();
            expectation e = new expectation();
            e.ExpectationMessage = errorMessage;
            e.UserName = username;
            e.StackTrace = strackTrace;
            e.dateLogedIn = DateTime.Now;
            e.InnerException = InnerException;
            e.GetClassName = GetClassName;
            e.URL = URL;

            
            db.expectations.Add(e);
            int i = db.SaveChanges();
            if (i > 0)
            {
                return "Error";
            }
            return "Error";
        }
    }
    
}