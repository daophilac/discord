package com.peanut.discord.customview;
import android.content.Context;
import android.util.AttributeSet;

import androidx.appcompat.widget.AppCompatImageView;

import com.peanut.discord.R;
public class EditImage extends AppCompatImageView {
    public EditImage(Context context) {
        super(context);
    }
    public EditImage(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.editImageStyle);
    }
    public EditImage(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
