// 
// (C) 2006-2007 The SharpOS Project Team (http://www.sharpos.org)
//
// Authors:
//	Mircea-Cristian Racasan <darx_kies@gmx.net>
//
// Licensed under the terms of the GNU GPL License version 2.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using SharpOS.AOT.IR;
using SharpOS.AOT.IR.Instructions;
using SharpOS.AOT.IR.Operands;
using SharpOS.AOT.IR.Operators;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;


namespace SharpOS.AOT.IR {
	public interface IAssembly {
		/// <summary>
		/// Encodes the specified engine.
		/// </summary>
		/// <param name="engine">The engine.</param>
		/// <param name="target">The target.</param>
		/// <returns></returns>
		bool Encode (Engine engine, string target);

		/// <summary>
		/// Gets the available registers count.
		/// </summary>
		/// <value>The available registers count.</value>
		int AvailableRegistersCount { get; }

		/// <summary>
		/// Spills the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		bool Spill (Operands.Operand.InternalSizeType type);

		/// <summary>
		/// Determines whether the specified value is register.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified value is register; otherwise, <c>false</c>.
		/// </returns>
		bool IsRegister (string value);

		/// <summary>
		/// Determines whether the specified value is instruction.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		/// 	<c>true</c> if the specified value is instruction; otherwise, <c>false</c>.
		/// </returns>
		bool IsInstruction (string value);

		/// <summary>
		/// Gets the type of the register size.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		SharpOS.AOT.IR.Operands.Operand.InternalSizeType GetRegisterSizeType (string value);
	}
}