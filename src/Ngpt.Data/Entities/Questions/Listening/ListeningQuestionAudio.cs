﻿using Augur.Entity.Base.Entities;
using Augur.Entity.Interfaces.Base;
using Ngpt.Platform.Data.Entities.Tenants;
using Ngpt.Platform.Data.Entities.UploadedResources;
using Ngpt.Platform.Data.Entities.Users;
using Ngpt.Platform.Data.Interfaces.Entities;
using System.Collections.Generic;

namespace Ngpt.Data.Entities.Questions.Listening
{
    public class ListeningQuestionAudio : AugurEntityWithId, ITenantEntity, IUserOwnedEntityWithUserId
    {
        public string Title { get; set; }
        public bool Approved { get; set; }

        public UploadedResource Resource { get; set; }
        public int? ResourceId { get; set; }

        public Language Language { get; set; }
        public int LanguageId { get; set; }

        public Level Level { get; set; }
        public int LevelId { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual Tenant Tenant { get; set; }
        public int TenantId { get; set; }

        public User Approver { get; set; }
        public int? ApproverId { get; set; }

        public ICollection<ListeningQuestion> Questions { get; set; }
    }
}
