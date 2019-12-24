using System;
using System.Linq;
using App.DL.Enum;
using Dapper;

namespace App.DL.Model.Funding {
    public class CurrencyWallet : Micron.DL.Model.Model {
        public int id;

        public string guid;

        public string address;

        public CurrencyType currency_type;

        public DateTime created_at;

        public DateTime updated_at;

        public static CurrencyWallet Find(int id)
            => Connection().Query<CurrencyWallet>(
                "SELECT * FROM currency_wallets WHERE id = @id LIMIT 1",
                new {id}
            ).FirstOrDefault();

        public static CurrencyWallet FindBy(string col, string val)
            => Connection().Query<CurrencyWallet>(
                $"SELECT * FROM currency_wallets WHERE {col} = @val LIMIT 1",
                new {val}
            ).FirstOrDefault();

        public static CurrencyWallet FindByEnum(string col, string val)
            => Connection().Query<CurrencyWallet>(
                $"SELECT * FROM currency_wallets WHERE {col} = '{val}' LIMIT 1",
                new {val}
            ).FirstOrDefault();

        public static CurrencyWallet FindRandomByEnum(string col, string val)
            => Connection().Query<CurrencyWallet>(
                $"SELECT * FROM currency_wallets WHERE {col} = '{val}' ORDER BY random() LIMIT 1",
                new {val}
            ).FirstOrDefault();

        public CurrencyWallet Refresh() => Find(id);

        public static int Create(string address, CurrencyType currencyType)
            => ExecuteScalarInt(
                $@"INSERT INTO currency_wallets(guid, address, currency_type) 
                        VALUES (@guid, @address, '{currencyType}'); 
                        SELECT currval('currency_wallets_id_seq');"
                , new {guid = Guid.NewGuid().ToString(), address}
            );
    }
}