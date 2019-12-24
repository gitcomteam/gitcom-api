using System;
using App.AL.Utils.Funding;
using App.DL.Enum;
using App.DL.Model.User;
using App.DL.Repository.Project;
using App.DL.Repository.Subscription;
using App.DL.Repository.User;

namespace App.AL.Utils.Subscription {
    public static class SubscriptionUtils {
        // TODO: add functionality based on user settings
        // TODO: add multiple ways to distribute funds
        public static bool PeriodPay(User user) {
            var subscriptionInfo = UserSubscriptionInfoRepository.FindOrCreate(user);

            var paydayTime = subscriptionInfo.last_paid.AddMonths(1);

            if (DateTime.Now < paydayTime) {
                return false;
            }
            
            var currency = subscriptionInfo.selected_currency;
            
            var balance = UserBalanceRepository.FindOrCreate(user, currency);

            decimal amount = 0;
            
            amount = balance.balance < subscriptionInfo.selected_amount ? balance.balance : subscriptionInfo.selected_amount;

            var randomProjectsToFund = 4; // TODO: get from user settings

            var amountToFundRandomProjects = amount / randomProjectsToFund;

            for (int i = 1; i <= randomProjectsToFund; i++) {
                var randomProject = ProjectRepository.FindRandom();
                SubscriptionFundingUtils.FundEntity(
                    user, randomProject.id, EntityType.Project, amountToFundRandomProjects, currency
                );
            }

            return true;
        }
    }
}