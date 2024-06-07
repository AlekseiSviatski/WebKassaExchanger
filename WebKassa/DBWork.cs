using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using WebKassa.Models.DBModel;

namespace WebKassa
{
    public class DBWork
    {
        private string _connectionString;

        public DBWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ICollection<SingleServiceDB>> GetSingleServices(int? id = null)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            var parametres = new Dictionary<string, object>
            {
                {"@pID", id }
            };
            var queryParams = new DynamicParameters(parametres);

            var singleServiceList = (await connection.QueryAsync<SingleServiceDB>("SingleServiceGet", queryParams, commandType: CommandType.StoredProcedure)).ToList();
            return singleServiceList;
        }

        public async Task<bool> UpdateSingleServicesSales(ICollection<SingleServiceUpdateModel> sales, int? idCashier, int? idCashbox, int? idEmployee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using DbTransaction transaction = await connection.BeginTransactionAsync();
                try
                {
                    sales.ToList()
                        .ForEach(s =>
                        {
                            int? ssuId = null;
                            int? chekListId = null;
                            int? cashboxPaymentId = null;

                            #region SSU

                            ssuId = SingleServiceUsedEdit(connection, transaction, s);

                            #endregion

                            #region CheckList

                            if (s.CashPrice > 0)
                            {
                                chekListId = CashboxCheckListEdit(connection, transaction,
                                new CashboxCheckListEditModel
                                {
                                    Price = s.CashPrice,
                                    TotalModey = s.CashPrice,
                                    IdCashier = idCashier, //из конфига
                                    CashboxId = idCashbox // из конфига
                                });
                            }

                            #endregion

                            #region CashboxPayment

                            if (s.CardPrice > 0)
                            {
                                cashboxPaymentId = CashboxPaymentEdit(connection, transaction,
                                new CashboxPaymentEditModel
                                {
                                    CashboxPaymentTypeId = 3,
                                    Price = s.CardPrice,
                                    IdCashier = idCashier, // из конфига
                                    CashboxId = idCashbox // из конфига
                                });
                            }

                            #endregion

                            #region PaymentHistory

                            if (chekListId != null)
                            {
                                PaymentHistoryEdit(connection, transaction,
                                    new PaymentHistoryEditModel
                                    {
                                        CheckListID = chekListId,
                                        CardPaymentId = null,
                                        CashlessPaymentID = null,
                                        CashboxID = idCashbox, // из конфига
                                        SingleServiceUsedID = ssuId,
                                        Count = (s.TotalPrice / s.CashPrice) * s.Count,
                                        PaymentPrice = s.CashPrice,
                                        Price = s.Price,
                                        TotalPrice = s.TotalPrice,
                                        EmployeeID = idEmployee, // из конфига
                                        CashierID = idCashier, // из конфига
                                    });
                            }

                            if (cashboxPaymentId != null)
                            {
                                PaymentHistoryEdit(connection, transaction,
                                    new PaymentHistoryEditModel
                                    {
                                        CheckListID = null,
                                        CardPaymentId = cashboxPaymentId,
                                        CashlessPaymentID = null,
                                        CashboxID = idCashbox, // из конфига
                                        SingleServiceUsedID = ssuId,
                                        Count = (s.TotalPrice / s.CardPrice) * s.Count,
                                        PaymentPrice = s.CardPrice,
                                        Price = s.Price,
                                        TotalPrice = s.TotalPrice,
                                        EmployeeID = idEmployee, // из конфига
                                        CashierID = idCashier, // из конфига
                                    });
                            }

                            #endregion
                        });

                    await transaction.CommitAsync();

                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();

                    return false;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

        private int? SingleServiceUsedEdit(SqlConnection connection, DbTransaction transaction, SingleServiceUpdateModel s)
        {
            try
            {
                var parametres = new Dictionary<string, object>
                            {
                                {"@pClientOrderID", s.IdOrder },
                                {"@pSingleServiceID", s.SingleServiceId },
                                {"@pCount", s.Count },
                                {"@pPrice", s.Price },
                                {"@pTotalPrice", s.TotalPrice },
                                {"@pDiscountPrice", s.DiscountPrice },
                                {"@pEmployeeOrderID", s.EmployeeOrderId },
                                {"@pSingleServiceResevedID", s.pSingleServiceResevedId },
                                {"@pPaid", s.Paid },
                                {"@pPaidPrice", s.PaidPrice },
                                {"@pPaidCount", s.PaidCount },
                                {"@pTerminalID", s.TerminalId },
                                {"@pSingleServiceQueueID", null },
                            };

                var dynamicParametres = new DynamicParameters(parametres);

                var ssuId = connection.Query<int?>("SingleServiceUsedEdit", dynamicParametres, transaction, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return ssuId;
            }
            catch
            {
                throw;
            }
        }

        private int? CashboxCheckListEdit(SqlConnection connection, DbTransaction transaction, CashboxCheckListEditModel c)
        {
            try
            {
                var parametres = new Dictionary<string, object>
                {
                    {"@pIdOrder", c.IdOrder},
                    {"@pIdSeasonOrder", c.IdSeasonOrder},
                    {"@pCashboxPaymentTypeID", c.CashboxPaymentTypeId},
                    {"@pPrice", c.Price},
                    {"@pTotalMoney", c.TotalModey},
                    {"@pTypeSystem", c.TypeSystem},
                    {"@pIdCashier", c.IdCashier},
                    {"@pIdMode", c.Mode},
                    {"@pCashboxID", c.CashboxId},
                };

                var dynamicParametres = new DynamicParameters(parametres);
                dynamicParametres.Add("@pIdCheck", null, DbType.Int32, ParameterDirection.InputOutput);

                var result = connection.Query("CashboxCheckListEdit", dynamicParametres, transaction, commandType: CommandType.StoredProcedure);

                int? idCheck = dynamicParametres.Get<int?>("@pIdCheck");

                return idCheck;
            }
            catch
            {
                throw;
            }
        }

        private int? CashboxPaymentEdit(SqlConnection connection, DbTransaction transaction, CashboxPaymentEditModel c)
        {
            try
            {
                var parametres = new Dictionary<string, object>
                {
                    {"@pCashboxPaymentTypeID", c.CashboxPaymentTypeId},
                    {"@pPrice", c.Price},
                    {"@pCodeAuthorization", c.CodeAuthorization},
                    {"@pCodeCard", c.CodeCard},
                    {"@pIdOrder", c.IdOrder},
                    {"@pIdSeasonOrder", c.IdSeasonOrder},
                    {"@pIdCashier", c.IdCashier},
                    {"@pCashboxID", c.CashboxId},
                    {"@pCardTypeID", c.CardTypeId},
                };

                var dynamicParametres = new DynamicParameters(parametres);
                dynamicParametres.Add("@pIdPayment", null, DbType.Int32, ParameterDirection.InputOutput);

                var result = connection.Query("CashboxPaymentEdit", dynamicParametres, transaction, commandType: CommandType.StoredProcedure);

                int? idPayment = dynamicParametres.Get<int?>("@pIdPayment");

                return idPayment;
            }
            catch
            {
                throw;
            }
        }

        private void PaymentHistoryEdit(SqlConnection connection, DbTransaction transaction, PaymentHistoryEditModel p)
        {
            try
            {
                var parametres = new Dictionary<string, object>
                {
                    {"@pMode", p.Mode},
                    {"@pID", p.Id},
                    {"@pCheckListID", p.CheckListID},
                    {"@pCardPaymentID", p.CardPaymentId},
                    {"@pCashlessPaymentID", p.CashlessPaymentID},
                    {"@pPaymentTypeID", p.PaymentTypeID},
                    {"@pCashboxID", p.CashboxID},
                    {"@pFiscalMode", p.FiscalMode},
                    {"@pCertificateID", p.CertificateID},
                    {"@pDiscountID", p.DiscountID},
                    {"@pMedServReservedID", p.MedServReservedID},
                    {"@pSingleServiceReservedID", p.SingleServiceReservedID},
                    {"@pSeasonOrderID", p.SeasonOrderID},
                    {"@pCertificateReservedID", p.CertificateReservedID},
                    {"@pJWSMoneyID", p.JWSMoneyID},
                    {"@pJWSSingleServiceID", p.JWSSingleServiceID},
                    {"@pCashboxOrderID", p.CashboxOrderID},
                    {"@pPayCashboxOrderID", p.PayCashboxOrderID},
                    {"@pSingleServiceUsedID", p.SingleServiceUsedID},
                    {"@pCount", p.Count},
                    {"@pPaymentPrice", p.PaymentPrice},
                    {"@pPrice", p.Price},
                    {"@pTotalPrice", p.TotalPrice},
                    {"@pBalanceTopUp", p.BalanceTopUp},
                    {"@pPaySeasonOrderID", p.PaySeasonOrderID},
                    {"@pEmployeeID", p.EmployeeID},
                    {"@pClientID", p.ClientID},
                    {"@pWriteOff", p.WriteOff},
                    {"@pRefund", p.Refund},
                    {"@pCashierID", p.CashierID},
                };

                var dynamicParametres = new DynamicParameters(parametres);

                connection.Query("PaymentHistoryEdit", dynamicParametres, transaction, commandType: CommandType.StoredProcedure);
            }
            catch
            {
                throw;
            }
        }
    }
}
