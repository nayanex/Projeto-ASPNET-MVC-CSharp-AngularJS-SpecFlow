using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.BLL.BOs
{
    public class SessionConnection : IDisposable
    {
        private Session session;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (session != null)
                {
                    session.Dispose();
                    session = null;
                }
        }
        ~SessionConnection()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Session getSession()
        {
            if (session == null)
            {
				session = new Session() { ConnectionString = @"Data source=localhost\SQLEXPRESS;initial catalog=wex;persist security info=True; Integrated Security=True; MultipleActiveResultSets=True" };
            }

            return session;
        }

    }
}