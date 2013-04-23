function EnhancedCategoriesPlugin() {
	this._settings = {
		expandText		: '&raquo;',
		contractText	: '&laquo;',
		leafText		: '-',
		expandImage		: '',	
		contractImage	: '',
		leafImage		: '',
		buttonColor		: '#CC0000',
		buttonWidth		: '10px',
		buttonMargin	: '0 5px 0 0',
		isButtonAfter	: false
	};
}

jQuery.extend(EnhancedCategoriesPlugin.prototype, {
	setSettings: function(newSettings) {
		jQuery.extend(this._settings, newSettings || {});
	},
	
	getSettings: function() {
		return this._settings;
	},
	
	getButtonText: function(text, image) {
		var output = '';
		if (image=='') {
			output += text;
		} else {
			output += '<img src="' + image + '" alt="' + text + '" title="' + text + '" />';
		}
		
		return output;
	}
});

jQuery(document).ready(function() {
	enhancedCategoriesPlugin = new EnhancedCategoriesPlugin();
});

jQuery.fn.enhancedCategories = function(args) { 
	var defaults = enhancedCategoriesPlugin.getSettings(); 
	jQuery.extend(defaults, args);
	
	return jQuery('li.cat-item', this).each(function() {	
		var childrenUl = jQuery(this).children('ul');
		var hasChildren = (childrenUl.length > 0);
		var button = '';
		
		// Add some padding to the RSS images
		jQuery('img', this)
			.css({	margin: '0 5px', 
					padding: '0'
				});

		// Make button text
		if (hasChildren) {
			if (defaults.contractChildren==1) {
				button += '<span class="button is_expanded" style="cursor: pointer;">';
				button += enhancedCategoriesPlugin.getButtonText(defaults.expandText, defaults.expandImage);
				button += '</span>';
			} else {
				button += '<span class="button is_contracted" style="cursor: pointer;">';
				button += enhancedCategoriesPlugin.getButtonText(defaults.contractText, defaults.contractImage);
				button += '</span>';
			}
		} else {
			button += '<span class="button" style="">';
			button += enhancedCategoriesPlugin.getButtonText(defaults.leafText, defaults.leafImage);
			button += '</span>';
		}
		
		// Add the button before or after the category
		if (defaults.isButtonAfter) {		
			if (hasChildren) {
				jQuery(this).children('ul').before(button);
			} else {
				jQuery(this).append(button);
			}			
		} else {
			jQuery(this).prepend(button);
		}
		
		// Behaviour of the category
		jQuery(this)
			.css({listStyleType: 'none'})
			.children('span.button')
				.css({ 	width: 	defaults.buttonWidth, 
						margin: defaults.buttonMargin, 
						color: 	defaults.buttonColor 
					})
				.click(function() {
						jQuery(this)
							.siblings('ul')
								.slideToggle()
							.end()
							.each(function() {
								if (jQuery(this).hasClass('is_expanded')) {
									jQuery(this)
										.html(enhancedCategoriesPlugin.getButtonText(defaults.contractText, defaults.contractImage))
										.removeClass('is_expanded')
										.addClass('is_contracted');
								} else {
									jQuery(this)
										.html(enhancedCategoriesPlugin.getButtonText(defaults.expandText, defaults.expandImage))
										.removeClass('is_contracted')
										.addClass('is_expanded');
								}
								return this;
							});
					});
					
		jQuery(this).children('ul')
			.css({ 	paddingLeft: defaults.buttonWidth });

		// Contract child categories if asked
		if (defaults.contractChildren==1) {
			jQuery(this).children('ul').hide();
		}

		return this;
	});
};