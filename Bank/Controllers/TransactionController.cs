using Bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly UserManager<Account> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly SignInManager<Account> _SigninManager;
        private readonly ApplicationDbContext _context;

        public TransactionController(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Account> signinManager, ApplicationDbContext context)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
            _SigninManager = signinManager;
            _context = context;
        }


        [HttpPost]
        [Route("deposit")]
        public async Task<IActionResult> Deposit(decimal amount, int userUniqueId)
        {


            if (amount <= 0)
            {
                var response = new
                {

                    Message = "Amount Should be Greater than 0",
                    IsSuccess = false
                };
                return BadRequest(response);
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(Account => Account.UniqueId == userUniqueId);

            if (account == null)
            {
                var response = new
                {
                    Message = "Account not found",
                    IsSuccess = false
                };
                return NotFound(response);
            }

            account.Balance += amount;


            var transaction = new Transaction
            {
                Amount = amount,
                AccountId = account.Id,
                Date = DateTime.Now,
                transactionType = TransactionType.Deposit
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            var successresponse = new
            {
                Message = "Deposit Successful",
                IsSuccess = true
            };
            return Ok(successresponse);
        }

        [HttpPost]
        [Route("withdraw")]
        public async Task<IActionResult> Withdraw(decimal amount, int userUniqueId)
        {
            if (amount <= 0)
            {
                var response = new
                {
                    Message = "Amount Should be Greater than 0",
                    IsSuccess = false
                };
                return BadRequest(response);
            }
            var account = await _context.Accounts.FirstOrDefaultAsync(Account => Account.UniqueId == userUniqueId);
            if (account == null)
            {
                var response = new
                {
                    Message = "Account not found",
                    IsSuccess = false
                };
                return NotFound(response);
            }
            if (account.Balance < amount)
            {
                var response = new
                {
                    Message = "Insufficient Balance",
                    IsSuccess = false
                };
                return BadRequest(response);
            }
            account.Balance -= amount;

            var transaction = new Transaction
            {
                Amount = amount,
                AccountId = account.Id,
                Date = DateTime.Now,
                transactionType = TransactionType.Withdrawal
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            var successresponse = new
            {
                Message = "Withdrawal Successful",
                IsSuccess = true
            };
            return Ok(successresponse);
        }

        [HttpPost]
        [Route("transfer")]
        public async Task<IActionResult> Transfer(decimal amount, int senderUniqueId, int receiverUniqueId)
        {
            if (amount <= 0)
            {
                var response = new
                {
                    Message = "Amount Should be Greater than 0",
                    IsSuccess = false
                };
                return BadRequest(response);
            }

            var senderAccount = await _context.Accounts.FirstOrDefaultAsync(Account => Account.UniqueId == senderUniqueId);
            var receiverAccount = await _context.Accounts.FirstOrDefaultAsync(Account => Account.UniqueId == receiverUniqueId);

            if (senderAccount == null || receiverAccount == null)
            {
                var response = new
                {
                    Message = "Account not found",
                    IsSuccess = false
                };
                return NotFound(response);
            }
            if (senderAccount.Balance < amount)
            {
                var response = new
                {
                    Message = "Insufficient Balance",
                    IsSuccess = false
                };
                return BadRequest(response);
            }
            senderAccount.Balance -= amount;
            receiverAccount.Balance += amount;



            var transactionsender = new Transaction
            {
                Amount = amount,
                AccountId = senderAccount.Id,
                RecipientId = receiverAccount.Id,
                transactionType = TransactionType.TransferTo,
                Date = DateTime.Now,
            };

            var transactionreceiver = new Transaction
            {
                Amount = amount,
                AccountId = receiverAccount.Id,
                RecipientId = senderAccount.Id,
                transactionType = TransactionType.TransferFrom,
                Date = DateTime.Now,
            };

            _context.Transactions.Add(transactionsender);
            _context.Transactions.Add(transactionreceiver);

            await _context.SaveChangesAsync();
            var successresponse = new
            {
                Message = "Transfer Successful",
                IsSuccess = true
            };
            return Ok(successresponse);
        }

        [HttpPost]
        [Route("loanrequest")]
        public async Task<IActionResult> LoanRequest(decimal amount, int userUniqueId)
        {
            if (amount <= 0)
            {
                var response = new
                {
                    Message = "Amount Should be Greater than 0",
                    IsSuccess = false
                };
                return BadRequest(response);
            }
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UniqueId == userUniqueId);
            if (account == null)
            {
                var response = new
                {
                    Message = "Account not found",
                    IsSuccess = false
                };
                return NotFound(response);
            }
            var loanRequest = new LoanRequest
            {
                Amount = amount,
                AccountId = account.Id,
                Status = LoanStatus.Pending,
            };
            _context.LoanRequests.Add(loanRequest);
            await _context.SaveChangesAsync();
            var successresponse = new
            {
                Message = "Loan Request Successful",
                IsSuccess = true
            };
            return Ok(successresponse);
        }

        [HttpGet]
        [Route("transactionhistory")]
        public async Task<IActionResult> TransactionHistory(int userUniqueId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UniqueId == userUniqueId);
            if (account == null)
            {
                var response = new
                {
                    Message = "Account not found",
                    IsSuccess = false
                };
                return NotFound(response);
            }
            var transactions = _context.Transactions
                                    .Where(t => t.Account.UniqueId == account.UniqueId)
                                      .Select(t => new
                                      {
                                          Amount = t.Amount,
                                          Type = t.transactionType,
                                          Date = t.Date,
                                          RecipientFullName = $"{t.Recipient.FirstName} {t.Recipient.LastName}"
                                      }).ToListAsync();




            var successresponse = new
            {
                Message = "Transaction History",
                IsSuccess = true,
                Payload = new
                {
                    Transactions = transactions
                }
            };
            return Ok(successresponse);
        }


        [HttpPost]
        [Route("loanrequestlist")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LoanRequestList()
        {
            var loanRequests = await _context.LoanRequests.Select(l => new
            {
                Id = l.Id,
                Amount = l.Amount,
                Status = l.Status,
                AccountUniqueId = l.Account.UniqueId,
            })
            .ToListAsync();
            var successresponse = new
            {
                Message = "Loan Request List",
                IsSuccess = true,
                Payload = new
                {
                    LoanRequests = loanRequests
                }
            };
            return Ok(successresponse);
        }


        [HttpPost]
        [Route("approveloan")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveLoan(Guid loanRequestId)
        {
            var loanRequest = await _context.LoanRequests.FirstOrDefaultAsync(l => l.Id == loanRequestId);
            if (loanRequest == null)
            {
                var response = new
                {
                    Message = "Loan Request not found",
                    IsSuccess = false
                };
                return NotFound(response);
            }
            loanRequest.Status = LoanStatus.Approved;
            _context.LoanRequests.Update(loanRequest);
            await _context.SaveChangesAsync();
            var successresponse = new
            {
                Message = "Loan Request Approved",
                IsSuccess = true
            };
            return Ok(successresponse);
        }

        [HttpPost]
        [Route("rejectloan")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectLoan(Guid loanRequestId)
        {
            var loanRequest = await _context.LoanRequests.FirstOrDefaultAsync(l => l.Id == loanRequestId);
            if (loanRequest == null)
            {
                var response = new
                {
                    Message = "Loan Request not found",
                    IsSuccess = false
                };
                return NotFound(response);
            }
            loanRequest.Status = LoanStatus.Rejected;
            _context.LoanRequests.Update(loanRequest);
            await _context.SaveChangesAsync();
            var successresponse = new
            {
                Message = "Loan Request Rejected",
                IsSuccess = true
            };
            return Ok(successresponse);
        }

    }
}
