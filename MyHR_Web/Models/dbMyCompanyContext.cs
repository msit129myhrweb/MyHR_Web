using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class dbMyCompanyContext : DbContext
    {
        public dbMyCompanyContext()
        {
        }

        public dbMyCompanyContext(DbContextOptions<dbMyCompanyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TAbsence> TAbsences { get; set; }
        public virtual DbSet<TApplyDetail> TApplyDetails { get; set; }
        public virtual DbSet<TBulletin> TBulletins { get; set; }
        public virtual DbSet<TCheckStatus> TCheckStatuses { get; set; }
        public virtual DbSet<TCostCategory> TCostCategories { get; set; }
        public virtual DbSet<TInterView> TInterViews { get; set; }
        public virtual DbSet<TInterViewProcess> TInterViewProcesses { get; set; }
        public virtual DbSet<TInterViewStatus> TInterViewStatuses { get; set; }
        public virtual DbSet<TLeave> TLeaves { get; set; }
        public virtual DbSet<TLeaveApplication> TLeaveApplications { get; set; }
        public virtual DbSet<TLostAndFound> TLostAndFounds { get; set; }
        public virtual DbSet<TLostAndFoundCategory> TLostAndFoundCategories { get; set; }
        public virtual DbSet<TLostAndFoundCheckStatus> TLostAndFoundCheckStatuses { get; set; }
        public virtual DbSet<TLostAndFoundSubject> TLostAndFoundSubjects { get; set; }
        public virtual DbSet<TRepair> TRepairs { get; set; }
        public virtual DbSet<TTravelExpenseApplication> TTravelExpenseApplications { get; set; }
        public virtual DbSet<TUser> TUsers { get; set; }
        public virtual DbSet<TUserDepartment> TUserDepartments { get; set; }
        public virtual DbSet<TUserJobTitle> TUserJobTitles { get; set; }
        public virtual DbSet<TUserOnBoardStatus> TUserOnBoardStatuses { get; set; }
        public virtual DbSet<TWuChaItem> TWuChaItems { get; set; }
        public virtual DbSet<TWuChaOrder> TWuChaOrders { get; set; }
        public virtual DbSet<TWuChaOrderStoreDetail> TWuChaOrderStoreDetails { get; set; }
        public virtual DbSet<TWuChaStore> TWuChaStores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=dbMyCompany;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<TAbsence>(entity =>
            {
                entity.HasKey(e => e.CApplyNumber)
                    .HasName("PK_Absence");

                entity.ToTable("tAbsence");

                entity.Property(e => e.CApplyNumber).HasColumnName("cApplyNumber");

                entity.Property(e => e.CEmployeeId).HasColumnName("cEmployeeID");

                entity.Property(e => e.COff)
                    .HasColumnType("datetime")
                    .HasColumnName("cOff");

                entity.Property(e => e.COn)
                    .HasColumnType("datetime")
                    .HasColumnName("cOn");

                entity.HasOne(d => d.CEmployee)
                    .WithMany(p => p.TAbsences)
                    .HasForeignKey(d => d.CEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tAbsence_tUser");
            });

            modelBuilder.Entity<TApplyDetail>(entity =>
            {
                entity.HasKey(e => e.CApplyNumber);

                entity.ToTable("tApplyDetail");

                entity.Property(e => e.CApplyNumber)
                    .ValueGeneratedNever()
                    .HasColumnName("cApplyNumber");

                entity.Property(e => e.CAmont)
                    .HasColumnType("money")
                    .HasColumnName("cAmont");

                entity.Property(e => e.CCostId).HasColumnName("cCostID");

                entity.HasOne(d => d.CCost)
                    .WithMany(p => p.TApplyDetails)
                    .HasForeignKey(d => d.CCostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tApplyDetail_tCostCategory");
            });

            modelBuilder.Entity<TBulletin>(entity =>
            {
                entity.HasKey(e => e.CNumber)
                    .HasName("PK_Bulletin");

                entity.ToTable("tBulletin");

                entity.Property(e => e.CNumber).HasColumnName("cNumber");

                entity.Property(e => e.CCategory)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("cCategory");

                entity.Property(e => e.CContentofBulletin)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("cContentofBulletin");

                entity.Property(e => e.CDepartment).HasColumnName("cDepartment");

                entity.Property(e => e.CEndtime)
                    .HasColumnType("datetime")
                    .HasColumnName("cEndtime");

                entity.Property(e => e.CStarttime)
                    .HasColumnType("datetime")
                    .HasColumnName("cStarttime");

                entity.Property(e => e.CTitle)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cTitle");

                entity.HasOne(d => d.CDepartmentNavigation)
                    .WithMany(p => p.TBulletins)
                    .HasForeignKey(d => d.CDepartment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bulletin_Department");
            });

            modelBuilder.Entity<TCheckStatus>(entity =>
            {
                entity.HasKey(e => e.CCheckStatusId)
                    .HasName("PK_CheckStatus");

                entity.ToTable("tCheckStatus");

                entity.Property(e => e.CCheckStatusId).HasColumnName("cCheckStatusID");

                entity.Property(e => e.CCheckStatus)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cCheckStatus");
            });

            modelBuilder.Entity<TCostCategory>(entity =>
            {
                entity.HasKey(e => e.CCostId)
                    .HasName("PK_Cost");

                entity.ToTable("tCostCategory");

                entity.Property(e => e.CCostId).HasColumnName("cCostID");

                entity.Property(e => e.CCostCategory)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cCostCategory");
            });

            modelBuilder.Entity<TInterView>(entity =>
            {
                entity.HasKey(e => e.CInterVieweeId)
                    .HasName("PK_InterViewee");

                entity.ToTable("tInterView");

                entity.Property(e => e.CInterVieweeId).HasColumnName("cInterVieweeID");

                entity.Property(e => e.CAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cAddress");

                entity.Property(e => e.CAge).HasColumnName("cAge");

                entity.Property(e => e.CBirthday)
                    .HasColumnType("date")
                    .HasColumnName("cBirthday");

                entity.Property(e => e.CDepartment).HasColumnName("cDepartment");

                entity.Property(e => e.CEducation)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cEducation");

                entity.Property(e => e.CEmail)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("cEmail");

                entity.Property(e => e.CEmployeeEnglishName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cEmployeeEnglishName");

                entity.Property(e => e.CExperience)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("cExperience");

                entity.Property(e => e.CInterViewDate)
                    .HasColumnType("date")
                    .HasColumnName("cInterViewDate");

                entity.Property(e => e.CInterViewProcessId).HasColumnName("cInterViewProcessID");

                entity.Property(e => e.CInterViewStatusId).HasColumnName("cInterViewStatusID");

                entity.Property(e => e.CInterVieweeGender)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("cInterVieweeGender");

                entity.Property(e => e.CInterVieweeName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cInterVieweeName");

                entity.Property(e => e.CInterViewerEmployeeId).HasColumnName("cInterViewerEmployeeID");

                entity.Property(e => e.CJobTitle).HasColumnName("cJobTitle");

                entity.Property(e => e.CPhone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cPhone");

                entity.Property(e => e.CPhoto).HasColumnName("cPhoto");

                entity.HasOne(d => d.CDepartmentNavigation)
                    .WithMany(p => p.TInterViews)
                    .HasForeignKey(d => d.CDepartment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterViewee_Department");

                entity.HasOne(d => d.CInterViewProcess)
                    .WithMany(p => p.TInterViews)
                    .HasForeignKey(d => d.CInterViewProcessId)
                    .HasConstraintName("FK_tInterView_tInterViewProcess");

                entity.HasOne(d => d.CInterViewStatus)
                    .WithMany(p => p.TInterViews)
                    .HasForeignKey(d => d.CInterViewStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterViewee_InterViewStatus");

                entity.HasOne(d => d.CInterViewerEmployee)
                    .WithMany(p => p.TInterViews)
                    .HasForeignKey(d => d.CInterViewerEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tInterView_tUser");

                entity.HasOne(d => d.CJobTitleNavigation)
                    .WithMany(p => p.TInterViews)
                    .HasForeignKey(d => d.CJobTitle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterViewee_JobTitle");
            });

            modelBuilder.Entity<TInterViewProcess>(entity =>
            {
                entity.HasKey(e => e.CInterViewProcessId);

                entity.ToTable("tInterViewProcess");

                entity.Property(e => e.CInterViewProcessId)
                    .ValueGeneratedNever()
                    .HasColumnName("cInterViewProcessID");

                entity.Property(e => e.CInterViewProcess)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("cInterViewProcess");
            });

            modelBuilder.Entity<TInterViewStatus>(entity =>
            {
                entity.HasKey(e => e.InterViewStatusId)
                    .HasName("PK_InterViewStatus_1");

                entity.ToTable("tInterViewStatus");

                entity.Property(e => e.InterViewStatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("InterViewStatusID");

                entity.Property(e => e.InterViewStatus)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TLeave>(entity =>
            {
                entity.HasKey(e => e.CLeaveId)
                    .HasName("PK_Leave");

                entity.ToTable("tLeave");

                entity.Property(e => e.CLeaveId).HasColumnName("cLeaveID");

                entity.Property(e => e.CLeaveCategory)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cLeaveCategory");
            });

            modelBuilder.Entity<TLeaveApplication>(entity =>
            {
                entity.HasKey(e => e.CApplyNumber)
                    .HasName("PK_LeaveApplication");

                entity.ToTable("tLeaveApplication");

                entity.Property(e => e.CApplyNumber).HasColumnName("cApplyNumber");

                entity.Property(e => e.CApplyDate)
                    .HasColumnType("datetime")
                    .HasColumnName("cApplyDate");

                entity.Property(e => e.CCheckStatus)
                    .HasColumnName("cCheckStatus")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CDepartmentId).HasColumnName("cDepartmentID");

                entity.Property(e => e.CEmployeeId).HasColumnName("cEmployeeID");

                entity.Property(e => e.CLeaveCategory).HasColumnName("cLeaveCategory");

                entity.Property(e => e.CLeaveEndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cLeaveEndTime");

                entity.Property(e => e.CLeaveStartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cLeaveStartTime");

                entity.Property(e => e.CReason)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cReason");

                entity.HasOne(d => d.CCheckStatusNavigation)
                    .WithMany(p => p.TLeaveApplications)
                    .HasForeignKey(d => d.CCheckStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeaveApplication_CheckStatus");

                entity.HasOne(d => d.CDepartment)
                    .WithMany(p => p.TLeaveApplications)
                    .HasForeignKey(d => d.CDepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tLeaveApplication_tUserDepartment");

                entity.HasOne(d => d.CEmployee)
                    .WithMany(p => p.TLeaveApplications)
                    .HasForeignKey(d => d.CEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tLeaveApplication_tUser");

                entity.HasOne(d => d.CLeaveCategoryNavigation)
                    .WithMany(p => p.TLeaveApplications)
                    .HasForeignKey(d => d.CLeaveCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeaveApplication_Leave");
            });

            modelBuilder.Entity<TLostAndFound>(entity =>
            {
                entity.HasKey(e => e.CPropertyId);

                entity.ToTable("tLostAndFound");

                entity.Property(e => e.CPropertyId).HasColumnName("cPropertyID");

                entity.Property(e => e.CDeparmentId).HasColumnName("cDeparmentID");

                entity.Property(e => e.CEmployeeId).HasColumnName("cEmployeeID");

                entity.Property(e => e.CLostAndFoundDate)
                    .HasColumnType("date")
                    .HasColumnName("cLostAndFoundDate");

                entity.Property(e => e.CLostAndFoundSpace)
                    .HasMaxLength(50)
                    .HasColumnName("cLostAndFoundSpace");

                entity.Property(e => e.CPhone)
                    .HasMaxLength(20)
                    .HasColumnName("cPhone");

                entity.Property(e => e.CProperty)
                    .HasMaxLength(50)
                    .HasColumnName("cProperty");

                entity.Property(e => e.CPropertyCategoryId).HasColumnName("cPropertyCategoryID");

                entity.Property(e => e.CPropertyCheckStatusId).HasColumnName("cPropertyCheckStatusID");

                entity.Property(e => e.CPropertyPhoto).HasColumnName("cPropertyPhoto");

                entity.Property(e => e.CPropertySubjectId).HasColumnName("cPropertySubjectID");

                entity.Property(e => e.CtPropertyDescription)
                    .HasColumnType("text")
                    .HasColumnName("ctPropertyDescription");

                entity.HasOne(d => d.CDeparment)
                    .WithMany(p => p.TLostAndFounds)
                    .HasForeignKey(d => d.CDeparmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tLostAndFound_tUserDepartment");

                entity.HasOne(d => d.CEmployee)
                    .WithMany(p => p.TLostAndFounds)
                    .HasForeignKey(d => d.CEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tLostAndFound_tUser");

                entity.HasOne(d => d.CPropertyCategory)
                    .WithMany(p => p.TLostAndFounds)
                    .HasForeignKey(d => d.CPropertyCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tLostAndFound_tLostAndFoundCategory");

                entity.HasOne(d => d.CPropertyCheckStatus)
                    .WithMany(p => p.TLostAndFounds)
                    .HasForeignKey(d => d.CPropertyCheckStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tLostAndFound_tLostAndFoundCheckStatus");

                entity.HasOne(d => d.CPropertySubject)
                    .WithMany(p => p.TLostAndFounds)
                    .HasForeignKey(d => d.CPropertySubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tLostAndFound_tLostAndFoundSubject");
            });

            modelBuilder.Entity<TLostAndFoundCategory>(entity =>
            {
                entity.HasKey(e => e.CPropertyCategoryId)
                    .HasName("PK_cLostAndFoundCategory");

                entity.ToTable("tLostAndFoundCategory");

                entity.Property(e => e.CPropertyCategoryId).HasColumnName("cPropertyCategoryID");

                entity.Property(e => e.CPropertyCategory)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("cPropertyCategory");
            });

            modelBuilder.Entity<TLostAndFoundCheckStatus>(entity =>
            {
                entity.HasKey(e => e.CcPropertyCheckStatusId);

                entity.ToTable("tLostAndFoundCheckStatus");

                entity.Property(e => e.CcPropertyCheckStatusId).HasColumnName("ccPropertyCheckStatusID");

                entity.Property(e => e.CcPropertyCheckStatus)
                    .HasMaxLength(15)
                    .HasColumnName("ccPropertyCheckStatus");
            });

            modelBuilder.Entity<TLostAndFoundSubject>(entity =>
            {
                entity.HasKey(e => e.CPropertySubjectId)
                    .HasName("PK_cLostAndFoundSubject");

                entity.ToTable("tLostAndFoundSubject");

                entity.Property(e => e.CPropertySubjectId).HasColumnName("cPropertySubjectID");

                entity.Property(e => e.CPropertySubject)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("cPropertySubject");
            });

            modelBuilder.Entity<TRepair>(entity =>
            {
                entity.HasKey(e => e.CRepairNumber)
                    .HasName("PK_Repair");

                entity.ToTable("tRepair");

                entity.Property(e => e.CRepairNumber).HasColumnName("cRepairNumber");

                entity.Property(e => e.CAppleDate)
                    .HasColumnType("date")
                    .HasColumnName("cAppleDate");

                entity.Property(e => e.CContentofRepair)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cContentofRepair");

                entity.Property(e => e.CEmployeeId).HasColumnName("cEmployeeID");

                entity.Property(e => e.CLocation)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cLocation");

                entity.Property(e => e.CPhone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cPhone");

                entity.Property(e => e.CRepairCategory)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cRepairCategory");

                entity.Property(e => e.CRepairStatus).HasColumnName("cRepairStatus");

                entity.HasOne(d => d.CEmployee)
                    .WithMany(p => p.TRepairs)
                    .HasForeignKey(d => d.CEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tRepair_tUser");
            });

            modelBuilder.Entity<TTravelExpenseApplication>(entity =>
            {
                entity.HasKey(e => e.CApplyNumber)
                    .HasName("PK_Travel_Expense_Application");

                entity.ToTable("tTravel_Expense_Application");

                entity.Property(e => e.CApplyNumber).HasColumnName("cApplyNumber");

                entity.Property(e => e.CAmont)
                    .HasColumnType("money")
                    .HasColumnName("cAmont");

                entity.Property(e => e.CApplyDate)
                    .HasColumnType("datetime")
                    .HasColumnName("cApplyDate");

                entity.Property(e => e.CCheckStatus)
                    .HasColumnName("cCheckStatus")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CDepartmentId).HasColumnName("cDepartmentID");

                entity.Property(e => e.CEmployeeId).HasColumnName("cEmployeeID");

                entity.Property(e => e.CReason)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cReason");

                entity.Property(e => e.CTravelEndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cTravelEndTime");

                entity.Property(e => e.CTravelStartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("cTravelStartTime");

                entity.HasOne(d => d.CCheckStatusNavigation)
                    .WithMany(p => p.TTravelExpenseApplications)
                    .HasForeignKey(d => d.CCheckStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tTravel_Expense_Application_tCheckStatus");

                entity.HasOne(d => d.CDepartment)
                    .WithMany(p => p.TTravelExpenseApplications)
                    .HasForeignKey(d => d.CDepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tTravel_Expense_Application_tUserDepartment");

                entity.HasOne(d => d.CEmployee)
                    .WithMany(p => p.TTravelExpenseApplications)
                    .HasForeignKey(d => d.CEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tTravel_Expense_Application_tUser");
            });

            modelBuilder.Entity<TUser>(entity =>
            {
                entity.HasKey(e => e.CEmployeeId)
                    .HasName("PK_User");

                entity.ToTable("tUser");

                entity.Property(e => e.CEmployeeId).HasColumnName("cEmployeeID");

                entity.Property(e => e.CAccountEnable).HasColumnName("cAccountEnable");

                entity.Property(e => e.CAddress)
                    .HasMaxLength(50)
                    .HasColumnName("cAddress");

                entity.Property(e => e.CBirthday)
                    .HasColumnType("date")
                    .HasColumnName("cBirthday");

                entity.Property(e => e.CByeByeDay)
                    .HasColumnType("date")
                    .HasColumnName("cByeByeDay");

                entity.Property(e => e.CDepartmentId).HasColumnName("cDepartmentID");

                entity.Property(e => e.CEmail)
                    .HasMaxLength(30)
                    .HasColumnName("cEmail");

                entity.Property(e => e.CEmergencyContact)
                    .HasMaxLength(20)
                    .HasColumnName("cEmergencyContact");

                entity.Property(e => e.CEmergencyPerson)
                    .HasMaxLength(20)
                    .HasColumnName("cEmergencyPerson");

                entity.Property(e => e.CEmployeeEnglishName)
                    .HasMaxLength(20)
                    .HasColumnName("cEmployeeEnglishName");

                entity.Property(e => e.CEmployeeName)
                    .HasMaxLength(20)
                    .HasColumnName("cEmployeeName");

                entity.Property(e => e.CGender)
                    .HasMaxLength(10)
                    .HasColumnName("cGender");

                entity.Property(e => e.CJobTitleId).HasColumnName("cJobTitleID");

                entity.Property(e => e.COnBoardDay)
                    .HasColumnType("date")
                    .HasColumnName("cOnBoardDay");

                entity.Property(e => e.COnBoardStatusId).HasColumnName("cOnBoardStatusID");

                entity.Property(e => e.CPassWord)
                    .HasMaxLength(20)
                    .HasColumnName("cPassWord");

                entity.Property(e => e.CPhone)
                    .HasMaxLength(20)
                    .HasColumnName("cPhone");

                entity.Property(e => e.CPhoto).HasColumnName("cPhoto");

                entity.Property(e => e.CSupervisor).HasColumnName("cSupervisor");

                entity.HasOne(d => d.COnBoardStatus)
                    .WithMany(p => p.TUsers)
                    .HasForeignKey(d => d.COnBoardStatusId)
                    .HasConstraintName("FK_tUser_tUserOnBoardStatus");
            });

            modelBuilder.Entity<TUserDepartment>(entity =>
            {
                entity.HasKey(e => e.CDepartmentId)
                    .HasName("PK_Department");

                entity.ToTable("tUserDepartment");

                entity.Property(e => e.CDepartmentId).HasColumnName("cDepartmentID");

                entity.Property(e => e.CDepartment)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cDepartment");
            });

            modelBuilder.Entity<TUserJobTitle>(entity =>
            {
                entity.HasKey(e => e.CJobTitleId)
                    .HasName("PK_JobTitle");

                entity.ToTable("tUserJobTitle");

                entity.Property(e => e.CJobTitleId).HasColumnName("cJobTitleID");

                entity.Property(e => e.CJobTitle)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cJobTitle");
            });

            modelBuilder.Entity<TUserOnBoardStatus>(entity =>
            {
                entity.HasKey(e => e.COnBoardStatusId)
                    .HasName("PK_OnBoardStatus");

                entity.ToTable("tUserOnBoardStatus");

                entity.Property(e => e.COnBoardStatusId).HasColumnName("cOnBoardStatusID");

                entity.Property(e => e.COnBoardStatus)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("cOnBoardStatus");
            });

            modelBuilder.Entity<TWuChaItem>(entity =>
            {
                entity.HasKey(e => e.CItemId)
                    .HasName("PK_Item");

                entity.ToTable("tWuChaItem");

                entity.Property(e => e.CItemId).HasColumnName("cItemID");

                entity.Property(e => e.CItemName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("cItemName");

                entity.Property(e => e.CItemPrice).HasColumnName("cItemPrice");

                entity.Property(e => e.CStoreId).HasColumnName("cStoreID");

                entity.HasOne(d => d.CStore)
                    .WithMany(p => p.TWuChaItems)
                    .HasForeignKey(d => d.CStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item_Store");
            });

            modelBuilder.Entity<TWuChaOrder>(entity =>
            {
                entity.HasKey(e => e.CWuChaOrderNumber);

                entity.ToTable("tWuChaOrder");

                entity.Property(e => e.CWuChaOrderNumber).HasColumnName("cWuChaOrderNumber");

                entity.Property(e => e.CDate)
                    .HasColumnType("date")
                    .HasColumnName("cDate");

                entity.Property(e => e.CEmployeeId).HasColumnName("cEmployeeID");

                entity.Property(e => e.CGroupId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("cGroupID")
                    .IsFixedLength(true);

                entity.Property(e => e.CStoreId).HasColumnName("cStoreID");

                entity.Property(e => e.CTotalPirce).HasColumnName("cTotalPirce");

                entity.HasOne(d => d.CEmployee)
                    .WithMany(p => p.TWuChaOrders)
                    .HasForeignKey(d => d.CEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tWuChaOrder_tUser");

                entity.HasOne(d => d.CStore)
                    .WithMany(p => p.TWuChaOrders)
                    .HasForeignKey(d => d.CStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WuChaOrder_Store");
            });

            modelBuilder.Entity<TWuChaOrderStoreDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tWuChaOrderStoreDetail");

                entity.Property(e => e.CItemId).HasColumnName("cItemID");

                entity.Property(e => e.CItemQuantity).HasColumnName("cItemQuantity");

                entity.Property(e => e.CStoreId).HasColumnName("cStoreID");

                entity.Property(e => e.CWuChaOrderNumber).HasColumnName("cWuChaOrderNumber");

                entity.HasOne(d => d.CItem)
                    .WithMany()
                    .HasForeignKey(d => d.CItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tWuChaOrderStoreDetail_tWuChaItem");

                entity.HasOne(d => d.CStore)
                    .WithMany()
                    .HasForeignKey(d => d.CStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tWuChaOrderStoreDetail_tWuChaStore");

                entity.HasOne(d => d.CWuChaOrderNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.CWuChaOrderNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tWuChaOrderStoreDetail_tWuChaOrder");
            });

            modelBuilder.Entity<TWuChaStore>(entity =>
            {
                entity.HasKey(e => e.CStoreId)
                    .HasName("PK_Store");

                entity.ToTable("tWuChaStore");

                entity.Property(e => e.CStoreId)
                    .ValueGeneratedNever()
                    .HasColumnName("cStoreID");

                entity.Property(e => e.CStoreName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("cStoreName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
