package com.peanut.discord.customview;

import android.content.Context;
import android.util.AttributeSet;

import com.peanut.discord.R;

public class NavigationButton extends android.support.v7.widget.AppCompatImageButton {
    public NavigationButton(Context context) {
        super(context);
    }

    public NavigationButton(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.navigationButtonStyle);
    }

    public NavigationButton(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
