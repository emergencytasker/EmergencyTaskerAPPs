using Microsoft.AspNetCore.Mvc;

namespace ETClient.Controllers
{
    public class CookieController
    {
        private readonly IHttpContextAccessor m_HttpContextAccessor;

        public CookieController(IHttpContextAccessor httpContextAccessor)
        {
            m_HttpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(String cookieName, String value, Int32? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (String.IsNullOrEmpty(cookieName))
            {
                return;
            }
            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            }
            else
            {
                option.Expires = DateTime.Now.AddDays(1);
            }
            if (m_HttpContextAccessor.HttpContext.Response != null)
            {
                if (String.IsNullOrEmpty(value))
                {
                    value = "null";
                }
                m_HttpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, value, option);
                return;
            }
        }

        public String ReadCookie(String cookieName)
        {
            if (String.IsNullOrEmpty(cookieName))
            {
                return null;
            }
            if (m_HttpContextAccessor.HttpContext.Request != null && !String.IsNullOrEmpty(m_HttpContextAccessor.HttpContext.Request.Cookies[cookieName]))
            {
                return m_HttpContextAccessor.HttpContext.Request.Cookies[cookieName];
            }
            return null;
        }
    }
}
