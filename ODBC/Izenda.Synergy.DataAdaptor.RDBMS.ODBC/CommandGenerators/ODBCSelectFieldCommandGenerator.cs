// ---------------------------------------------------------------------- 
// <copyright file="ODBCSelectFieldCommandGenerator.cs" company="Izenda">
//  Copyright (c) 2015 Izenda, Inc.                          
//  ALL RIGHTS RESERVED                
//                                                                         
//  The entire contents of this file is protected by U.S. and      
//  International Copyright Laws. Unauthorized reproduction,        
//  reverse-engineering, and distribution of all or any portion of  
//  the code contained in this file is strictly prohibited and may  
//  result in severe civil and criminal penalties and will be      
//  prosecuted to the maximum extent possible under the law.        
//                                                                  
//  RESTRICTIONS                                                    
//                                                                  
//  THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES          
//  ARE CONFIDENTIAL AND PROPRIETARY TRADE                          
//  SECRETS OF IZENDA INC. THE REGISTERED DEVELOPER IS  
//  LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    
//  CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                
//                                                                  
//  THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      
//  FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        
//  COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE      
//  AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  
//  AND PERMISSION FROM IZENDA INC.                      
//                                                                  
//  CONSULT THE END USER LICENSE AGREEMENT(EULA FOR INFORMATION ON  
//  ADDITIONAL RESTRICTIONS.
// </copyright> 
// ----------------------------------------------------------------------



using Izenda.BI.Framework.Models.DBStructure;
using Izenda.BI.Framework.Utility;
using Izenda.BI.Utility.Reflections;
using System;
using System.Collections.Generic;
using System.Text;
using Izenda.BI.Utility.List;
using Izenda.BI.Framework.Constants;
using Izenda.BI.Framework.Components.QueryExpressionTree.Operator;
using Izenda.BI.Framework.Components.QueryExpressionTree;
using Izenda.BI.DataAdaptor.RDBMS.Constants;
using Izenda.BI.Framework.Components.ExpressionEvaluations.Functions;

namespace Izenda.BI.DataAdaptor.RDBMS.CommandGenerators
{
    /// <summary>
    /// Generate command for operand
    /// </summary>
    public abstract class ODBCSelectFieldCommandGenerator : ConditionalOperatorCommandGenerator
    {
        /// <summary>
        /// Contructor
        /// </summary>
        public ODBCSelectFieldCommandGenerator(QueryTreeCommandGeneratorVisitor visitor) : base(visitor)
        {
        }

        /// <summary>
        /// Expressions the command generator.
        /// </summary>
        /// <returns></returns>
        public virtual ExpressionCommandGenerator ExpressionCommandGenerator
        {
            get
            {
                var expressionCommandGenerator = (ExpressionCommandGenerator)TypeUtil.GetAssignableInstanceFromAssembly(this.GetType(), typeof(ExpressionCommandGenerator));
                expressionCommandGenerator.SupportedFunctions = visitor.ContextData.AllSupportedFunctions;
                expressionCommandGenerator.Parameters = visitor.ContextData.Parameters;
                return expressionCommandGenerator;
            }
        }

        /// <summary>
        /// Generate query for operand
        /// </summary>
        /// <param name="operand">The operand</param>
        /// <param name="childCommand">The child command</param>
        /// <returns>The query</returns>
        public string GenerateCommand(string childCommand, List<ReportField> fields, QueryTreeNode operatorNode, bool isTopProjection = true, bool includeAllFieldsFromChild = false, bool performCalculatedField = true)
        {
            var selectFields = new StringBuilder();
            var expressionCommandGenerator = ExpressionCommandGenerator;
            string pagingCommand = string.Empty;
            var query = @"SELECT {0}{2} FROM ({1}) X";

            bool isOperandProjection = IsOperandProjection(operatorNode);
            bool isOperandGrouping = IsOperandGrouping(operatorNode);
            bool isAdjustTreeGrouping = IsAdjustTreeGroupingForDrilldown(operatorNode);

            if (includeAllFieldsFromChild)
            {
                query = @"SELECT X.* {0}{2} FROM ({1}) X";
            }

            // add row number & total records
            bool hasPaging = this.visitor.ContextData.Paging.PageIndex > 0 && this.visitor.ContextData.Paging.PageSize > 0;
            if (isTopProjection && hasPaging && visitor.HasPaging)
            {
                pagingCommand = PlaceHolder.PagingField;
            }

            var processedFieldAlias = new List<string>();

            foreach (var field in fields)
            {
                bool hasDefaultPercentFormat = Formating.DefaultPercentFormatIds.Contains(field.Formating?.Format ?? string.Empty);

                if (field.IsRunningField)
                {
                    continue;
                }

                var fieldAlias = $"{FieldUtil.GenerateFieldAlias(field, visitor.ContextData)}";

                if (!isTopProjection)
                {
                    //For projection node but does not handle calculated field
                    //we will generate the alias base on table and relationship.
                    //The alias base on field name alias is used only for top level projection
                    var fieldName = visitor.ContextData.Fields[field.FieldId].Name;
                    fieldAlias = FieldUtil.GenerateFieldAlias(fieldName, field.FieldId, field.SourceAlias);
                }

                if (!hasDefaultPercentFormat)
                {
                    if (!processedFieldAlias.AddIfNotExist(fieldAlias))
                    {
                        continue;
                    }
                }

                if (field.CalculatedTree != null && isTopProjection && performCalculatedField)
                {
                    if (field.NonAggregatedFunction && operatorNode is GroupingOperator)
                    {
                        selectFields.AppendFormat(@"{0}{1}{2},", DatabaseEscape.FieldEscapeStart, fieldAlias, DatabaseEscape.FieldEscpaseEnd);
                        continue;
                    }

                    string commandExpression = expressionCommandGenerator.GenerateCommand(field.CalculatedTree);

                    // in case operatorNode is GroupingOperator
                    // still keep operandname
                    bool replaceFieldNameInExpression = hasDefaultPercentFormat &&
                                                        !(isOperandProjection || isOperandGrouping || isAdjustTreeGrouping);
                    if (replaceFieldNameInExpression)
                    {
                        // replace operand fieldname by fieldname has default total formatting
                        commandExpression = commandExpression.Replace(GetFieldNameWithoutFormatting(field), fieldAlias);
                    }

                    // in case userdefined function 
                    // keep name without format
                    if (field.CalculatedTree is UserDefinedToken && !field.IsCalculated)
                    {
                        selectFields.AppendFormat(@"{0} AS ""{1}"",", commandExpression, FieldUtil.GenerateFieldAlias(field, visitor.ContextData, withFormat: false));
                    }
                    else
                    {
                        selectFields.AppendFormat(@"{0} AS ""{1}"",", commandExpression, fieldAlias);
                    }
                }
                else
                {
                    if (hasDefaultPercentFormat)
                    {
                        if (isOperandProjection || isOperandGrouping)
                        {
                            string operandFieldName = GetFieldNameWithoutFormatting(field);

                            // field with formatting                       
                            selectFields.AppendFormat(
                                @"{0}{1}{2} AS ""{3}"",",
                                DatabaseEscape.FieldEscapeStart,
                                operandFieldName,
                                DatabaseEscape.FieldEscpaseEnd,
                                FieldUtil.GenerateFieldAlias(field, visitor.ContextData));

                            // field without formatting
                            // normal field
                            if (!processedFieldAlias.Contains(operandFieldName) && !includeAllFieldsFromChild)
                            {
                                selectFields.AppendFormat(@"{0}{1}{2},", DatabaseEscape.FieldEscapeStart, operandFieldName, DatabaseEscape.FieldEscpaseEnd);
                                processedFieldAlias.Add(operandFieldName);
                            }
                        }
                        else
                        {
                            if (operatorNode is ProjectionOperator)
                            {
                                var projectionNode = ((ProjectionOperator)operatorNode);
                                if (!projectionNode.ParentOfDataSourceJoin)
                                {
                                    selectFields.AppendFormat(@"{0}{1}{2},", DatabaseEscape.FieldEscapeStart, fieldAlias, DatabaseEscape.FieldEscpaseEnd);
                                }
                            }
                            else
                            {
                                selectFields.AppendFormat(@"{0}{1}{2},", DatabaseEscape.FieldEscapeStart, fieldAlias, DatabaseEscape.FieldEscpaseEnd);
                            }
                        }
                    }
                    else
                    {
                        selectFields.AppendFormat(@"{0}{1}{2},", DatabaseEscape.FieldEscapeStart, fieldAlias, DatabaseEscape.FieldEscpaseEnd);
                    }
                }
            }

            if (!string.IsNullOrEmpty(pagingCommand))
            {
                visitor.TempData[QueryTreeCommandGeneratorKey.TempDataSelectedFields] = processedFieldAlias;
                visitor.TempData[QueryTreeCommandGeneratorKey.TempPerformCalculatedField] = performCalculatedField;
            }

            return string.Format(query, GetSelectFields(selectFields, includeAllFieldsFromChild), childCommand, pagingCommand);
        }        

        /// <summary>
        /// Gets the field name without formatting.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        protected string GetFieldNameWithoutFormatting(ReportField field)
        {
            var fieldName = visitor.ContextData.Fields[field.FieldId].Name;
            var operandFieldName = FieldUtil.GenerateFieldAlias(fieldName, field.FieldId, field.SourceAlias);
            return operandFieldName;
        }

        /// <summary>
        /// Determines whether [is grouping adjust tree for drilldown] [the specified operator node].
        /// </summary>
        /// <param name="operatorNode">The operator node.</param>
        /// <returns>
        ///   <c>true</c> if [is grouping adjust tree for drilldown] [the specified operator node]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsAdjustTreeGroupingForDrilldown(QueryTreeNode operatorNode)
        {
            if (operatorNode == null)
            {
                return false;
            }

            if (!(operatorNode is GroupingOperator))
            {
                return false;
            }

            var groupingNode = (GroupingOperator)operatorNode;

            if (!(groupingNode.ChildOperand is JoinOperator))
            {
                return false;
            }

            var joinNode = (JoinOperator)groupingNode.ChildOperand;

            return joinNode.LeftOperation != null && joinNode.LeftOperation is Operand;
        }

        /// <summary>
        /// Determines whether [is operand projection] [the specified operator node].
        /// </summary>
        /// <param name="operatorNode">The operator node.</param>
        /// <returns>
        ///   <c>true</c> if [is operand projection] [the specified operator node]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsOperandProjection(QueryTreeNode operatorNode)
        {
            if (operatorNode == null)
            {
                return false;
            }

            if (!(operatorNode is ProjectionOperator))
            {
                return false;
            }

            var projectionNode = (ProjectionOperator)operatorNode;

            if (projectionNode.ChildOperand is Operand)
            {
                return true;
            }

            if (!(projectionNode.ChildOperand is JoinOperator))
            {
                return false;
            }

            var joinNode = (JoinOperator)projectionNode.ChildOperand;

            return joinNode.LeftOperation != null && joinNode.LeftOperation is Operand;
        }

        /// <summary>
        /// Determines whether [is grouping operand] [the specified operator node].
        /// </summary>
        /// <param name="operatorNode">The operator node.</param>
        protected bool IsOperandGrouping(QueryTreeNode operatorNode)
        {
            if (operatorNode == null)
            {
                return false;
            }

            if (!(operatorNode is GroupingOperator))
            {
                return false;
            }

            var groupingNode = ((GroupingOperator)operatorNode);

            // check whether node directParantOfOperand
            // in drilldown grid
            if (groupingNode.ChildOperand is Operand)
            {
                return true;
            }

            // check whether node indirectParantOfOperand
            // in pivot grid
            if (!(groupingNode.ChildOperand is ProjectionOperator))
            {
                return false;
            }

            var projectionNode = (ProjectionOperator)groupingNode.ChildOperand;

            if (!(projectionNode?.ChildOperand is Operand))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the select fields.
        /// </summary>
        /// <param name="selectFields">The select fields.</param>
        /// <returns></returns>
        private string GetSelectFields(StringBuilder selectFields, bool includeAllFieldsFromChild)
        {
            if (selectFields.Length == 0)
            {
                return string.Empty;
            }

            if (includeAllFieldsFromChild)
            {
                return $", {selectFields.ToString().TrimEnd(',')}";
            }

            return $"{selectFields.ToString().TrimEnd(',')}";
        }
    }
}
