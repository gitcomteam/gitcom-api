using App.AL.Utils.Entity;
using App.DL.Enum;
using App.DL.Model.User;
using App.DL.Repository.Board;
using App.DL.Repository.BoardColumn;
using App.DL.Repository.Project;
using App.DL.Repository.ProjectTeamMember;
using App.DL.Repository.Card;

namespace App.AL.Utils.Permission {
    public static class PermissionUtils {
        public static bool HasEntityPermission(User user, int entityId, EntityType entityType) {
            if (!EntityUtils.IsEntityExists(entityId, entityType)) {
                return false;
            }
            switch (entityType) {
                case EntityType.Project:
                    var project = ProjectRepository.Find(entityId);
                    if (project != null && ProjectTeamMemberRepository.IsExists(project, user)) {
                        return true;
                    }
                    break;
                case EntityType.Board:
                    var board = BoardRepository.Find(entityId);
                    if (board != null && ProjectTeamMemberRepository.IsExists(board.Project(), user)) {
                        return true;
                    }
                    break;
                case EntityType.BoardColumn:
                    var column = BoardColumnRepository.Find(entityId);
                    if (column != null && ProjectTeamMemberRepository.IsExists(column.Board().Project(), user)) {
                        return true;
                    }
                    break;
                case EntityType.Card:
                    var card = CardRepository.Find(entityId);
                    // TODO: optimize?
                    if (card != null && ProjectTeamMemberRepository.IsExists(card.Column().Board().Project(), user)) {
                        return true;
                    }
                    break;
            }
            return false;
        }

        public static bool HasEntityPermission(User user, string entityGuid, EntityType entityType) {
            return HasEntityPermission(
                user,
                EntityUtils.GetEntityId(entityGuid, entityType),
                entityType
            );
        }
    }
}