using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrueLayer_NitinJain.Interfaces;
using TrueLayer_NitinJain.Model;

namespace TrueLayer_NitinJain.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountService;

        public AccountsController(IAccountsService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Gives account details by account id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult GetAccountById(string id)           
        {
            return RequireAuth(_accountService.GetAccount(id));
        }

        /// <summary>
        /// This will give all account details
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult GetAccounts()
        {
            return RequireAuth(_accountService.GetAccounts());
        }

        /// <summary>
        /// Gives the transaction details for given account id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/transactions")]
        public ActionResult GetTransactionsByAccountId(string id)
        {
            return RequireAuth(_accountService.GetTransactionsOnAccount(id));
        }

        /// <summary>
        /// this will give a shanp shot of transaction for an account, getting TransactionDetailResponse to have required details..
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/transactions/snapshot")]
        public ActionResult GetTransactionsSnapshotByAccountId(string id)
        {
            return RequireAuthTransactionResponse(_accountService.GetTransactionsMinMaxDetails(id).Result);
        }

        /// <summary>
        /// Redirects to Log In if response returns a 401.
        /// </summary>
        private ActionResult RequireAuth(HttpResponseMessage response)
        {
            if ((int)response.StatusCode != 200)
            {
                return RedirectToAction("LogIn", "Auth");
            }
            string jsonString = response.Content.ReadAsStringAsync().Result;
            
            return Content(jsonString, "application/json");
        }

        private ActionResult RequireAuthTransactionResponse(TransactionDetailResponse response)
        {
            if (response == null)
            {
                return RedirectToAction("LogIn", "Auth");
            }
            return Content( JsonConvert.SerializeObject(response));
        }
    }
}