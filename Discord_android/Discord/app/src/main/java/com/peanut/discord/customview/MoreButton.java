package com.peanut.discord.customview;

import android.content.Context;
import android.support.v7.widget.AppCompatImageButton;
import android.util.AttributeSet;

import com.peanut.discord.R;

public class MoreButton extends AppCompatImageButton {
    public MoreButton(Context context) {
        super(context);
    }

    public MoreButton(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.moreButtonStyle);
    }

    public MoreButton(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
