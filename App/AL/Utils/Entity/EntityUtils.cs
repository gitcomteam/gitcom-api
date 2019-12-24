using System.Linq;
using App.DL.Enum;
using Micron.DL.Module.Db;
using Dapper;

namespace App.AL.Utils.Entity {
    public static class EntityUtils {
        public static int GetEntityId(string guid, EntityType type) {
            string table = GetTableForEntityType(type);

            if (GetTableForEntityType(type) != null) {
                return DbConnection.Connection()
                    .Query<int>($"SELECT id FROM {table} where guid = @guid", new {guid}).FirstOrDefault();
            }

            return 0;
        }

        public static string GetEntityGuid(int id, EntityType type) {
            string table = GetTableForEntityType(type);

            if (table != null) {
                return DbConnection.Connection()
                    .Query<string>($"SELECT guid FROM {table} where id = @id", new {id}).FirstOrDefault();
            }

            return "";
        }

        public static bool IsEntityExists(string guid, EntityType type) {
            return IsEntityExists(GetEntityId(guid, type), type);
        }

        public static bool IsEntityExists(int id, EntityType type) {
            string table = GetTableForEntityType(type);

            if (table == null) {
                return false;
            }

            return DbConnection.Connection().ExecuteScalar<int>(
                       $"SELECT COUNT(*) FROM {table} where id = @id LIMIT 1", new {id}
                   ) > 0;
        }

        private static string GetTableForEntityType(EntityType type) {
            // TODO: add more entities
            switch (type) {
                case EntityType.Project:
                    return "projects";
                case EntityType.Board:
                    return "boards";
                case EntityType.Card:
                    return "cards";
                case EntityType.ProjectCategory:
                    return "project_categories";
                case EntityType.UserBalance:
                    return "user_balances";
                case EntityType.User:
                    return "users";
            }

            return null;
        }
    }
}