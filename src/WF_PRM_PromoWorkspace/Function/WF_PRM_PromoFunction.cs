//------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//------------------------------------------------------------

namespace oxxo
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Functions.Extensions.Workflows;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.Workflows.RuleEngine;
    using Microsoft.Extensions.Logging;
    using System.Xml;

    /// <summary>
    /// Represents the WF_PRM_PromoFunction flow invoked function.
    /// </summary>
    public class WF_PRM_PromoFunction
    {
        private readonly ILogger<WF_PRM_PromoFunction> logger;

        public WF_PRM_PromoFunction(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<WF_PRM_PromoFunction>();
        }

        /// <summary>
        /// Executes the logic app workflow.
        /// </summary>
        /// <param name="ruleSetName">The rule set name.</param>
        /// <param name="documentType">document type of input xml.</param>
        /// <param name="inputXml">input xml type fact</param>
        [FunctionName("WF_PRM_PromoFunction")]
        public Task<RuleExecutionResult> RunRules(
            [WorkflowActionTrigger] string ruleSetName, 
            string documentType, 
            string inputXml)
        {
            /***** Summary of steps below *****
             * 1. Get the rule set to Execute 
             * 2. Check if the rule set was retrieved successfully
             * 3. create the rule engine object
             * 4. Create TypedXmlDocument facts for all xml document facts
             * 5. Initialize .NET facts
             * 6. Execute rule engine
             * 7. Retrieve relevant updates facts and send them back
             */
            
            try
            {
                // Get the ruleset based on ruleset name
                var ruleExplorer = new FileStoreRuleExplorer();
                var ruleSet = ruleExplorer.GetRuleSet(ruleSetName);

                // Check if ruleset exists
                if(ruleSet == null)
                {
                    // Log an error in finding the rule set
                    this.logger.LogCritical($"RuleSet instance for '{ruleSetName}' was not found(null)");
                    throw new Exception($"RuleSet instance for '{ruleSetName}' was not found.");
                }             

                // Create rule engine instance
                var ruleEngine = new RuleEngine(ruleSet: ruleSet);

                // Create a typedXml Fact(s) from input xml(s)
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(inputXml);
                var typedXmlDocument = new TypedXmlDocument(documentType, doc);

                // Initialize .NET facts
                // Placeholder

                // Provide facts to rule engine and run it
                ruleEngine.Execute(new object[] { typedXmlDocument });

                // Send the relevant results(facts) back
                var updatedDoc = typedXmlDocument.Document as XmlDocument;
                var ruleExectionOutput = new RuleExecutionResult()
                {
                    XmlDoc = updatedDoc.OuterXml,
                    RuleSetName = ruleSetName,
                };

                return Task.FromResult(ruleExectionOutput);
            }
            catch(RuleEngineException ruleEngineException)
            {
                // Log any rule engine exceptions
                this.logger.LogCritical(ruleEngineException.ToString());
                throw;
            }
        }

        /// <summary>
        /// Results of the rule execution
        /// </summary>
        public class RuleExecutionResult
        {
            /// <summary>
            /// rules updated xml document
            /// </summary>
            public string XmlDoc { get; set;}

            /// <summary>
            /// RuleSet Name
            /// </summary>
            public string RuleSetName { get; set;}
        }
    }
}