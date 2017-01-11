﻿using System.Collections.Generic;

namespace Common.Dto
{
    /// <summary>
    /// Just a marker interface at the moment
    /// </summary>
    public interface IRequestable
    {
		//name, id, serviceId

		//help tip?

		ICollection<ITextInput> TextInputs { get; set; }
	//	ICollection<IScriptedSelection> ScriptedSelecentionInputs { get; set; }
	//	ICollection<ISelection> SelectionInputs { get; set; }

		//anything else required to be requestable. included items?
    }
}
