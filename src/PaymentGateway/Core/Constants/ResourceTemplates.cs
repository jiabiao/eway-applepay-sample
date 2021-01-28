// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

namespace MonkeyStore.PaymentGateway.Constants
{
    public static class ResourceTemplates
    {
        /// <summary>
        ///  Create AccessCode for Transparent Redirect.
        /// </summary>
        public const string TEMPLATE_ACCESS_CODES = "{Endpoint}/AccessCodes";

        /// <summary>
        /// Create AccessCode for Shared Page
        /// </summary>
        public const string TEMPLATE_ACCESS_CODES_SHARED = "{Endpoint}/AccessCodesShared";

        /// <summary>
        /// Create transaction by RAPID v5
        /// </summary>
        public const string TEMPLATE_TRANSACTION_V5 = "{Endpoint}/Transactions";

        /// <summary>
        /// Create transaction by Transparent Redirect.
        /// </summary>
        public const string TEMPLATE_TRANSACTION_TR = "{Endpoint}/AccessCode";

        /// <summary>
        /// RAPID v5 transaction query
        /// </summary>
        public const string TEMPLATE_TRANSACTION_V5_BY_ID = "{Endpoint}/Transactions/{TransactionId}";

        /// <summary>
        /// Query transaction by transaction ID.
        /// </summary>
        public const string TEMPLATE_TRANSACTION_BY_ID = "{Endpoint}/Transaction/{TransactionId}";

        /// <summary>
        /// Query transaction by AccessCode.
        /// </summary>
        public const string TEMPLATE_TRANSACTION_BY_ACCESS_CODE = "{Endpoint}/AccessCode/{AccessCode}";

        //Param Names for IUriComposer
        public const string PARAM_NAME_ACCESS_CODE = "{AccessCode}";
        public const string PARAM_NAME_TRANSACTION_ID = "{TransactionId}";
        public const string PARAM_NAME_ENDPOINT = "{Endpoint}";
    }
}
