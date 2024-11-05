namespace DiiaClient.SDK.Models.Remote
{
    public class DiiaIDAction
    {
        public const string AUTH = "auth";
        public const string HASHED_FILES_SIGNING = "hashedFilesSigning";
    }

    /// <summary>
    /// List of all supported document types.
    /// Refer the documentation to understand the operations can be done for each of them
    /// </summary>
    public static class DocumentTypeConstant
    {
        /// <summary>
        /// Паспорт громадянина України у формі ID-картки АБО біометричний закордонний паспорт
        ///(залежно від того, що є в користувача в Дії, передається лише один з документів)
        ///(рекомендований спосіб, коли треба отримати цифрову копію будь-якого паспорта клієнта).
        /// Either citizen passport in ID-Card form or biometric foreign passport.
        /// It's a recommended way to obtain a passport when it's type doesn't matter.
        /// </summary>
        public const string DOC_TYPE_PASSPORT = "passport";

        /// <summary>
        /// Паспорт громадянина України у формі ID-картки.
        /// Citizen passport in ID-Card form
        /// </summary>
        public const string DOC_TYPE_INTERNAL_PASSPORT = "internal-passport";

        /// <summary>
        /// Біометричний закордонний паспорт або закордонний паспорт.
        /// Biometric foreign passport or simple foreign passport
        /// </summary>
        public const string DOC_TYPE_FOREIGN_PASSPORT = "foreign-passport";

        /// <summary>
        /// РНОКПП.
        /// Taxpayer card
        /// </summary>
        public const string DOC_TYPE_TAXPAYER_CARD = "taxpayer-card";

        /// <summary>
        /// Довідка внутрішньо переміщеної особи (ВПО).
        /// Internally displaced person certificate
        /// </summary>
        public const string DOC_TYPE_REFERENCE_INTERNALLY_DISPLACED_PERSON = "reference-internally-displaced-person";

        /// <summary>
        /// Свідоцтво про народження дитини.
        /// Child's birth certificate
        /// </summary>
        public const string DOC_TYPE_BIRTH_CERTIFICATE = "birth-certificate";

        /// <summary>
        /// Посвідчення водія.
        /// Driver license
        /// </summary>
        public const string DOC_TYPE_DRIVER_LICENSE = "driver-license";

        /// <summary>
        /// Тех паспорт.
        /// Vehicle license
        /// </summary>
        public const string DOC_TYPE_VEHICLE_LICENSE = "vehicle-license";
    }
}
