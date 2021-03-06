﻿using System;
using Huobi.SDK.Core;
using Huobi.SDK.Core.Client;
using Huobi.SDK.Log;
using Huobi.SDK.Model.Request;

namespace Huobi.SDK.Example
{
    public class WalletClientExample
    {
        private static PerformanceLogger _logger = PerformanceLogger.GetInstance();

        public static void RunAll()
        {
            GetDepoistAddress();

            GetSubUserDepositAddress();

            GetWithdrawQuota();

            WithdrawCurrency();

            CancelWithdrawCurrency();

            GetDepositWithdrawHistory();

            GetSubUserDepositHistory();
        }

        private static void GetDepoistAddress()
        {
            var walletClient = new WalletClient(Config.AccessKey, Config.SecretKey);

            _logger.Start();
            var request = new GetRequest()
                .AddParam("currency", "btc");

            var result = walletClient.GetDepositAddressAsync(request).Result;
            _logger.StopAndLog();

            if (result != null && result.data != null)
            {
                AppLogger.Info($"Get deposit address, id={result.data.Length}");
                foreach (var a in result.data)
                {
                    AppLogger.Info($"currency: {a.currency}, addr: {a.address}, chain: {a.chain}");
                }
            }
        }

        private static void GetSubUserDepositAddress()
        {
            var walletClient = new WalletClient(Config.AccessKey, Config.SecretKey);

            _logger.Start();
            var result = walletClient.GetSubUserDepositAddressAsync(Config.SubUserId, "btc").Result;
            _logger.StopAndLog();

            if (result != null)
            {
                if (result.data != null)
                {
                    AppLogger.Info($"Get sub user deposit address, id={result.data.Length}");
                    foreach (var a in result.data)
                    {
                        AppLogger.Info($"currency: {a.currency}, addr: {a.address}, chain: {a.chain}");
                    }
                }
                else
                {
                    AppLogger.Error($"Get sub user deposit address error: code={result.code}, message={result.message}");
                }
            }
        }

        private static void GetWithdrawQuota()
        {
            var walletClient = new WalletClient(Config.AccessKey, Config.SecretKey);

            _logger.Start();
            var request = new GetRequest()
                .AddParam("currency", "btc");

            var result = walletClient.GetWithdrawQuotaAsync(request).Result;
            _logger.StopAndLog();

            if (result != null && result.data != null && result.data.chains != null)
            {
                foreach (var c in result.data.chains)
                {
                    AppLogger.Info($"chain: {c.chain}, max withdraw amount {c.maxWithdrawAmt}, total quota {c.withdrawQuotaTotal}");
                }
            }
        }

        private static void WithdrawCurrency()
        {
            var walletClient = new WalletClient(Config.AccessKey, Config.SecretKey);

            _logger.Start();
            var request = new WithdrawRequest
            {
                address = ""
            };
            var result = walletClient.WithdrawCurrencyAsync(request).Result;
            _logger.StopAndLog();

            if (result != null)
            {
                switch (result.status)
                {
                    case "ok":
                        {
                            AppLogger.Info($"Withdraw successfully, transfer id: {result.data}");
                            break;
                        }
                    case "error":
                        {
                            AppLogger.Info($"Withdraw fail, error code: {result.errorCode}, error message: {result.errorMessage}");
                            break;
                        }
                }
            }
        }

        private static void CancelWithdrawCurrency()
        {
            var walletClient = new WalletClient(Config.AccessKey, Config.SecretKey);

            _logger.Start();
            var result = walletClient.CancelWithdrawCurrencyAsync(1).Result;
            _logger.StopAndLog();

            if (result != null)
            {
                switch (result.status)
                {
                    case "ok":
                        {
                            AppLogger.Info($"Cancel withdraw successfully, transfer id: {result.data}");
                            break;
                        }
                    case "error":
                        {
                            AppLogger.Info($"Cancel withdraw fail, error code: {result.errorCode}, error message: {result.errorMessage}");
                            break;
                        }
                }
            }
        }

        private static void GetDepositWithdrawHistory()
        {
            var walletClient = new WalletClient(Config.AccessKey, Config.SecretKey);

            _logger.Start();
            var request = new GetRequest()
                    .AddParam("type", "deposit");
            var result = walletClient.GetDepositWithdrawHistoryAsync(request).Result;
            _logger.StopAndLog();

            if (result != null && result.data != null)
            {
                foreach (var h in result.data)
                {
                    AppLogger.Info($"type: {h.type}, currency: {h.currency}, amount: {h.amount}, updatedAt: {h.updatedAt}");
                }

                AppLogger.Info($"There are {result.data.Length} deposit and withdraw history");
            }
        }

        private static void GetSubUserDepositHistory()
        {
            var walletClient = new WalletClient(Config.AccessKey, Config.SecretKey);

            _logger.Start();
            var request = new GetRequest()
                    .AddParam("subUid", Config.SubUserId);

            var result = walletClient.GetSubUserDepositHistoryAsync(request).Result;
            _logger.StopAndLog();

            if (result != null && result.data != null)
            {
                AppLogger.Info($"Get sub user deposit history, count={result.data.Length}");
                foreach (var h in result.data)
                {
                    AppLogger.Info($"Deposit history, id={h.id}, currency={h.currency}, amount={h.amount}, address={h.address}, updatedAt={h.updateTime}");
                }
            }
        }
    }
}
